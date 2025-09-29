using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Windowing;

partial class Clipboard
{
	private sealed class DataGetterWrapper(DataGetter dataGetter) : IDisposable
	{
		private volatile DataGetter? mDataGetter = dataGetter;
		private volatile List<(byte[] data, MemoryHandle pin)>? mRetainedData = [];

		~DataGetterWrapper() => DisposeImpl();

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		public unsafe static void* DataCallback(void* userdata, byte* mime_type, nuint* size)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: DataGetterWrapper { mDataGetter: { } dataGetter, mRetainedData: { } retainedData } })
			{
				var mimeTypeUtf16 = Utf8StringMarshaller.ConvertToManaged(mime_type);

				var data = dataGetter(mimeTypeUtf16);

				if (data is not null)
				{
					var pin = data.AsMemory().Pin();

					retainedData.Add((data, pin));

					*size = unchecked((ulong)data.LongLength) <= nuint.MaxValue
						? unchecked((nuint)data.LongLength)
						: int.MaxValue;

					return pin.Pointer;
				}
			}

			*size = 0;

			return null;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		public unsafe static void CleanupCallback(void* userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: DataGetterWrapper wrapper } gcHandle)
			{
				wrapper.Dispose();

				gcHandle.Free();
			}
		}

		public void Dispose()
		{
			DisposeImpl();
			GC.SuppressFinalize(this);
		}

		private void DisposeImpl()
		{
			if (mDataGetter is not null)
			{				
				mDataGetter = null;
			}

			if (mRetainedData is not null)
			{
				foreach (var (_, pin) in mRetainedData)
				{
					pin.Dispose();
				}

				mRetainedData.Clear();

				mRetainedData = null;
			}
		}
	}
}
