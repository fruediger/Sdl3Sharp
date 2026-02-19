using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;

namespace Sdl3Sharp.Utilities;

public static partial class FileDialog
{
	public static void ShowWithProperties(FileDialogType type, FileDialogCallback callback, Sdl3Sharp.Properties properties, ReadOnlySpan<FileDialogFilter> filters = default, Window? parentWindow = null)
		=> ShowWithPropertiesImpl(type, callback, properties, filters.GetSpanEnumerator(), filters.Length, parentWindow);

	public static void ShowWithProperties<TFilters>(FileDialogType type, FileDialogCallback callback, Sdl3Sharp.Properties properties, [AllowNull] in TFilters? filters, Window? parentWindow = null)
		where TFilters : IReadOnlyCollection<FileDialogFilter>, allows ref struct
	{
		var (filtersEnumerator, filtersCount) = filters is not null
			? (filters.GetEnumerator(), filters.Count)
			: (null, 0);

		ShowWithPropertiesImpl(type, callback, properties, filtersEnumerator, filtersCount, parentWindow);
	}

	public static Task<(string[]? files, int filterIndex)> ShowWithPropertiesAsync(FileDialogType type, Sdl3Sharp.Properties properties, ReadOnlySpan<FileDialogFilter> filters = default, Window? parentWindow = null)
	{
		var tcs = new TaskCompletionSource<(string[]? files, int filterIndex)>();

		ShowWithProperties(type, (files, filterIndex) => tcs.SetResult((files, filterIndex)), properties, filters, parentWindow);

		return tcs.Task;
	}

	public static Task<(string[]? files, int filterIndex)> ShowWithPropertiesAsync<TFilters>(FileDialogType type, Sdl3Sharp.Properties properties, [AllowNull] in TFilters? filters, Window? parentWindow = null)
		where TFilters : IReadOnlyCollection<FileDialogFilter>, allows ref struct
	{
		var tcs = new TaskCompletionSource<(string[]? files, int filterIndex)>();

		ShowWithProperties(type, (files, filterIndex) => tcs.SetResult((files, filterIndex)), properties, filters, parentWindow);

		return tcs.Task;
	}

	private unsafe static void ShowWithPropertiesImpl<TFiltersEnumerator>(FileDialogType type, FileDialogCallback callback, Sdl3Sharp.Properties properties, [AllowNull] TFiltersEnumerator? filtersEnumerator, int filtersCount, Window? parentWindow)
		where TFiltersEnumerator : IEnumerator<FileDialogFilter>, allows ref struct
	{

		var wrapper = FileDialogCallbackWrapper.Create(callback, filtersEnumerator, ref filtersCount, out var filters);		

		try
		{
			bool filtersLocallySet, windowLocallySet;

			if (properties.Id is not 0)
			{
				filtersLocallySet = !properties.Contains(PropertyNames.FiltersPointer) && !properties.Contains(PropertyNames.NFiltersNumber);

				if (filtersLocallySet)
				{
					properties.TrySetPointerValue(PropertyNames.FiltersPointer, unchecked((IntPtr)filters));
					properties.TrySetNumberValue(PropertyNames.NFiltersNumber, filtersCount);
				}

				windowLocallySet = !properties.Contains(PropertyNames.WindowPointer);

				if (windowLocallySet)
				{
					properties.TrySetPointerValue(PropertyNames.WindowPointer, unchecked((IntPtr)(parentWindow is not null ? parentWindow.WindowPtr : null)));
				}
			}
			else
			{
				filtersLocallySet = false;
				windowLocallySet = false;
			}

			try
			{
				SDL_ShowFileDialogWithProperties(
					type,
					&DialogFileCallback,
					unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(wrapper, GCHandleType.Normal))),
					properties.Id
				);
			}
			finally
			{
				if (windowLocallySet)
				{
					properties.TryRemove(PropertyNames.WindowPointer);
				}

				if (filtersLocallySet)
				{
					properties.TryRemove(PropertyNames.NFiltersNumber);
					properties.TryRemove(PropertyNames.FiltersPointer);
				}
			}
		}
		catch
		{
			wrapper.Dispose();

			throw;
		}
	}

	public static void ShowOpenFile(FileDialogCallback callback, ReadOnlySpan<FileDialogFilter> filters = default, string? defaultLocation = default, bool allowMany = false, Window? parentWindow = null)
		=> ShowOpenFileImpl(callback, filters.GetSpanEnumerator(), filters.Length, defaultLocation, allowMany, parentWindow);

	public static void ShowOpenFile<TFilters>(FileDialogCallback callback, [AllowNull] in TFilters? filters, string? defaultLocation = default, bool allowMany = false, Window? parentWindow = null)
		where TFilters : IReadOnlyCollection<FileDialogFilter>, allows ref struct
	{
		var (filtersEnumerator, filtersCount) = filters is not null
			? (filters.GetEnumerator(), filters.Count)
			: (null, 0);

		ShowOpenFileImpl(callback, filtersEnumerator, filtersCount, defaultLocation, allowMany, parentWindow);
	}

	public static Task<(string[]? files, int filterIndex)> ShowOpenFileAsync(ReadOnlySpan<FileDialogFilter> filters = default, string? defaultLocation = default, bool allowMany = false, Window? parentWindow = null)
	{
		var tcs = new TaskCompletionSource<(string[]? files, int filterIndex)>();

		ShowOpenFile((files, filterIndex) => tcs.SetResult((files, filterIndex)), filters, defaultLocation, allowMany, parentWindow);

		return tcs.Task;
	}

	public static Task<(string[]? files, int filterIndex)> ShowOpenFileAsync<TFilters>([AllowNull] in TFilters? filters, string? defaultLocation = default, bool allowMany = false, Window? parentWindow = null)
		where TFilters : IReadOnlyCollection<FileDialogFilter>, allows ref struct
	{
		var tcs = new TaskCompletionSource<(string[]? files, int filterIndex)>();

		ShowOpenFile((files, filterIndex) => tcs.SetResult((files, filterIndex)), filters, defaultLocation, allowMany, parentWindow);

		return tcs.Task;
	}

	private unsafe static void ShowOpenFileImpl<TFiltersEnumerator>(FileDialogCallback callback, [AllowNull] TFiltersEnumerator? filtersEnumerator, int filtersCount, string? defaultLocation, bool allowMany, Window? parentWindow)
		where TFiltersEnumerator : IEnumerator<FileDialogFilter>, allows ref struct
	{
		var wrapper = FileDialogCallbackWrapper.Create(callback, filtersEnumerator, ref filtersCount, out var filters);

		try
		{
			var defaultLocationUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(defaultLocation);

			try
			{
				SDL_ShowOpenFileDialog(
					&DialogFileCallback,
					unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(wrapper, GCHandleType.Normal))),
					parentWindow is not null ? parentWindow.WindowPtr : null,
					filters,
					filtersCount,
					defaultLocationUtf8,
					allowMany
				);
			}
			finally
			{
				Utf8StringMarshaller.Free(defaultLocationUtf8);
			}

		}
		catch
		{
			wrapper.Dispose();

			throw;
		}
	}

	public static void ShowOpenFolder(FileDialogCallback callback, string? defaultLocation = default, bool allowMany = false, Window? parentWindow = null)
	{
		unsafe
		{
			var filtersCount = 0;
			var wrapper = FileDialogCallbackWrapper.Create<IEnumerator<FileDialogFilter>>(callback, null, ref filtersCount, out _);

			try
			{
				var defaultLocationUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(defaultLocation);

				try
				{
					SDL_ShowOpenFolderDialog(
						&DialogFileCallback,
						unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(wrapper, GCHandleType.Normal))),
						parentWindow is not null ? parentWindow.WindowPtr : null,
						defaultLocationUtf8,
						allowMany
					);
				}
				finally
				{
					Utf8StringMarshaller.Free(defaultLocationUtf8);
				}
			}
			catch
			{
				wrapper.Dispose();

				throw;
			}
		}
	}

	public static Task<string[]?> ShowOpenFolderAsync(string? defaultLocation = default, bool allowMany = false, Window? parentWindow = null)
	{		
		var tcs = new TaskCompletionSource<string[]?>();

		ShowOpenFolder((files, _) => tcs.SetResult(files), defaultLocation, allowMany, parentWindow);

		return tcs.Task;
	}

	public static void ShowSaveFile(FileDialogCallback callback, ReadOnlySpan<FileDialogFilter> filters = default, string? defaultLocation = default, Window? parentWindow = null)
		=> ShowSaveFileImpl(callback, filters.GetSpanEnumerator(), filters.Length, defaultLocation, parentWindow);

	public static void ShowSaveFile<TFilters>(FileDialogCallback callback, [AllowNull] in TFilters? filters, string? defaultLocation = default, Window? parentWindow = null)
		where TFilters : IReadOnlyCollection<FileDialogFilter>, allows ref struct
	{
		var (filtersEnumerator, filtersCount) = filters is not null
			? (filters.GetEnumerator(), filters.Count)
			: (null, 0);

		ShowSaveFileImpl(callback, filtersEnumerator, filtersCount, defaultLocation, parentWindow);
	}

	public static Task<(string? file, int filterIndex)> ShowSaveFileAsync(ReadOnlySpan<FileDialogFilter> filters = default, string? defaultLocation = default, Window? parentWindow = null)
	{
		var tcs = new TaskCompletionSource<(string? file, int filterIndex)>();

		ShowSaveFile((files, filterIndex) => tcs.SetResult((files is { Length: > 0 } ? files[0] : null, filterIndex)), filters, defaultLocation, parentWindow);

		return tcs.Task;
	}

	public static Task<(string? file, int filterIndex)> ShowSaveFileAsync<TFilters>([AllowNull] in TFilters? filters, string? defaultLocation = default, Window? parentWindow = null)
		where TFilters : IReadOnlyCollection<FileDialogFilter>, allows ref struct
	{
		var tcs = new TaskCompletionSource<(string? file, int filterIndex)>();

		ShowSaveFile((files, filterIndex) => tcs.SetResult((files is { Length: > 0 } ? files[0] : null, filterIndex)), filters, defaultLocation, parentWindow);

		return tcs.Task;
	}

	private unsafe static void ShowSaveFileImpl<TFiltersEnumerator>(FileDialogCallback callback, [AllowNull] TFiltersEnumerator? filtersEnumerator, int filtersCount, string? defaultLocation, Window? parentWindow)
		where TFiltersEnumerator : IEnumerator<FileDialogFilter>, allows ref struct
	{
		var wrapper = FileDialogCallbackWrapper.Create(callback, filtersEnumerator, ref filtersCount, out var filters);

		try
		{
			var defaultLocationUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(defaultLocation);

			try
			{
				SDL_ShowSaveFileDialog(
					&DialogFileCallback,
					unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(wrapper, GCHandleType.Normal))),
					parentWindow is not null ? parentWindow.WindowPtr : null,
					filters,
					filtersCount,
					defaultLocationUtf8
				);
			}
			finally
			{
				Utf8StringMarshaller.Free(defaultLocationUtf8);
			}

		}
		catch
		{
			wrapper.Dispose();

			throw;
		}
	}
}
