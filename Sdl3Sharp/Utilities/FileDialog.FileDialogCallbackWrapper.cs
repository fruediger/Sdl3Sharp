using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Utilities;

partial class FileDialog
{
	private sealed class FileDialogCallbackWrapper : IDisposable
	{
		private FileDialogCallback? mCallback;
		private unsafe SDL_DialogFileFilter* mFilters;
		private int mFiltersCount;

		private unsafe FileDialogCallbackWrapper(FileDialogCallback callback, SDL_DialogFileFilter* filters, int filtersCount)
		{ mCallback = callback; mFilters = filters; mFiltersCount = filtersCount; }

		~FileDialogCallbackWrapper() => DisposeImpl();

		public void Dispose()
		{
			DisposeImpl();
			GC.SuppressFinalize(this);
		}

		private unsafe void DisposeImpl()
		{
			if (mFilters is not null)
			{
				while (mFiltersCount is > 0)
				{
					var filter = mFilters[--mFiltersCount];

						Utf8StringMarshaller.Free(filter.Pattern);
						Utf8StringMarshaller.Free(filter.Name);
				}

				NativeMemoryManager.Free(mFilters);

				mFilters = null;
			}

			mCallback = null;
		}

		public unsafe void Invoke(byte** filelist, int filterIndex)
		{
			if (mCallback is not null)
			{
				string[]? files;

				if (filelist is not null)
				{
					var filePtr = filelist;
					var fileCount = 0;

					while (*filePtr++ is not null) { fileCount++; }

					if (fileCount is > 0)
					{
						files = GC.AllocateUninitializedArray<string>(fileCount);

						filePtr = filelist;
						foreach (ref var file in files.AsSpan())
						{
							file = Utf8StringMarshaller.ConvertToManaged(*filePtr++);
						}
					}
					else
					{
						files = [];
					}					
				}
				else
				{
					files = null;
				}

				mCallback(files, filterIndex);
			}
		}

		public unsafe static FileDialogCallbackWrapper Create<TFiltersEnumerator>(FileDialogCallback callback, [AllowNull] TFiltersEnumerator? filtersEnumerator, ref int filtersCount, out SDL_DialogFileFilter* filters)
			where TFiltersEnumerator : IEnumerator<FileDialogFilter>, allows ref struct
		{
			if (filtersEnumerator is not null && filtersCount is > 0)
			{
				filters = NativeMemoryManager.Malloc<SDL_DialogFileFilter>(unchecked((nuint)filtersCount));
				filtersCount = 0;

				if (filters is not null)
				{
					using (filtersEnumerator)
					{
						while (filtersEnumerator.MoveNext())
						{
							var (name, pattern) = filtersEnumerator.Current;

							filters[filtersCount++] = new() { Name = Utf8StringMarshaller.ConvertToUnmanaged(name), Pattern = Utf8StringMarshaller.ConvertToUnmanaged(pattern) };
						}
					}
				}
			}
			else
			{
				filters = null;
				filtersCount = 0;
			}

			try
			{
				return new FileDialogCallbackWrapper(callback, filters, filtersCount);
			}
			catch
			{
				if (filters is not null)
				{
					while (filtersCount is > 0)
					{
						var filter = filters[--filtersCount];

						Utf8StringMarshaller.Free(filter.Pattern);
						Utf8StringMarshaller.Free(filter.Name);
					}

					NativeMemoryManager.Free(filters);

					filters = null;
				}

				throw;
			}
		}
	}
}
