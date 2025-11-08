using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe SDL_IOStreamInterface_size = delegate* unmanaged[Cdecl]<void*, long>;
using unsafe SDL_IOStreamInterface_seek = delegate* unmanaged[Cdecl]<void*, long, Sdl3Sharp.IO.StreamWhence, long>;
using unsafe SDL_IOStreamInterface_read = delegate* unmanaged[Cdecl]<void*, void*, nuint, Sdl3Sharp.IO.StreamStatus*, nuint>;
using unsafe SDL_IOStreamInterface_write = delegate* unmanaged[Cdecl]<void*, void*, nuint, Sdl3Sharp.IO.StreamStatus*, nuint>;
using unsafe SDL_IOStreamInterface_flush = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.IO.StreamStatus*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_IOStreamInterface_close = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.Internal.Interop.CBool>;

namespace Sdl3Sharp.IO;

partial class Stream
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_IOStream;

	[StructLayout(LayoutKind.Sequential)]
	internal unsafe readonly struct SDL_IOStreamInterface
	{
		public readonly uint Version;
		public readonly SDL_IOStreamInterface_size Size;
		public readonly SDL_IOStreamInterface_seek Seek;
		public readonly SDL_IOStreamInterface_read Read;
		public readonly SDL_IOStreamInterface_write Write;
		public readonly SDL_IOStreamInterface_flush Flush;
		public readonly SDL_IOStreamInterface_close Close;

		public SDL_IOStreamInterface(
			SDL_IOStreamInterface_size size,
			SDL_IOStreamInterface_seek seek,
			SDL_IOStreamInterface_read read,
			SDL_IOStreamInterface_write write,
			SDL_IOStreamInterface_flush flush,
			SDL_IOStreamInterface_close close
		)
		{
			this = default; // make sure we're zeroed

			Version = unchecked((uint)Unsafe.SizeOf<SDL_IOStreamInterface>());
			Size = size;
			Seek = seek;
			Read = read;
			Write = write;
			Flush = flush;
			Close = close;
		}

		public SDL_IOStreamInterface(Stream stream, out GCHandle streamHandle) : this(
			&SizeImpl,
			&SeekImpl,
			&ReadImpl,
			&WriteImpl,
			&FlushImpl,
			&CloseImpl
		)
		{
			streamHandle = GCHandle.Alloc(stream, GCHandleType.Normal);
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static long SizeImpl(void* userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Stream stream })
			{
				return stream.LengthCore;
			}

			return -1; // return -1 => error
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static long SeekImpl(void* userdata, long offset, StreamWhence whence)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Stream stream })
			{
				return stream.SeekCore(offset, whence);
			}

			return -1; // return -1 => error
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static nuint ReadImpl(void* userdata, void* ptr, nuint size, StreamStatus* status)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Stream stream })
			{
				return stream.ReadCore(new Utilities.NativeMemory(ptr, size), ref Unsafe.AsRef<StreamStatus>(status));
			}

			*status = StreamStatus.Error;

			return 0; // status == error + return number of bytes read == 0 => error
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static nuint WriteImpl(void* userdata, void* ptr, nuint size, StreamStatus* status)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Stream stream })
			{
				return stream.WriteCore(new Utilities.NativeMemory(ptr, size), ref Unsafe.AsRef<StreamStatus>(status));
			}

			*status = StreamStatus.Error;

			return 0; // status == error + return number of bytes written == 0 => error
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool FlushImpl(void* userdata, StreamStatus* status)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Stream stream })
			{
				return stream.FlushCore(ref Unsafe.AsRef<StreamStatus>(status));
			}

			*status = StreamStatus.Error;

			return false; // status == error + return false => error
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool CloseImpl(void* userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Stream stream })
			{
				return stream.CloseCore();
			}

			return false; // return false => error
		}
	}

	/// <summary>
	/// Close and free an allocated <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure
	/// </summary>
	/// <param name="context"><see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure to close</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>() closes and cleans up the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> stream.
	/// It releases any resources used by the stream and frees the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> itself.
	/// This returns true on success, or false if the stream failed to flush to its output (e.g. to disk). 
	/// </para>
	/// <para>
	/// Note that if this fails to flush the stream for any reason, this function reports an error, but the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> is still invalid once this function returns.
	/// </para>
	/// <para>
	/// This call flushes any buffered writes to the operating system, but there are no guarantees that those writes have gone to physical media; they might be in the OS's file cache, waiting to go to disk later.
	/// If it's absolutely crucial that writes go to disk immediately, so they are definitely stored even if the power fails before the file cache would have caught up, one should call <see href="https://wiki.libsdl.org/SDL3/SDL_FlushIO">SDL_FlushIO</see>() before closing.
	/// Note that flushing takes time and makes the system and your app operate less efficiently, so do so sparingly.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CloseIO(SDL_IOStream* context);

	/// <summary>
	/// Flush any buffered data in the stream
	/// </summary>
	/// <param name="context"><see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure to flush</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function makes sure that any buffered data is written to the stream.
	/// Normally this isn't necessary but if the stream is a pipe or socket it guarantees that any pending data is sent.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_FlushIO">SDL_FlushIO</see>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_FlushIO(SDL_IOStream* context);

	/// <summary>
	/// Get the properties associated with an SDL_IOStream
	/// </summary>
	/// <param name="context">A pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetIOProperties">SDL_GetIOProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetIOProperties(SDL_IOStream* context);

	/// <summary>
	/// Use this function to get the size of the data stream in an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="context">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to get the size of the data stream from</param>
	/// <returns>
	/// Returns the size of the data stream in the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> on success or a negative error code on failure;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetIOSize">SDL_GetIOSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial long SDL_GetIOSize(SDL_IOStream* context);

	/// <summary>
	/// Query the stream status of an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="context">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to query</param>
	/// <returns>Returns an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStatus">SDL_IOStatus</see> enum with the current state</returns>
	/// <remarks>
	/// <para>
	/// This information can be useful to decide if a short read or write was due to an error, an EOF, or a non-blocking operation that isn't yet ready to complete.
	/// </para>
	/// <para>
	/// An <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>'s status is only expected to change after a <see href="https://wiki.libsdl.org/SDL3/SDL_ReadIO">SDL_ReadIO</see>
	/// or <see href="https://wiki.libsdl.org/SDL3/SDL_WriteIO">SDL_WriteIO</see> call; don't expect it to change if you just call this query function in a tight loop.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial StreamStatus SDL_GetIOStatus(SDL_IOStream* context);

	/// <summary>
	/// Print to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> data stream
	/// </summary>
	/// <param name="context">A pointer to an SDL_IOStream structure</param>
	/// <param name="fmt">A printf() style format string</param>
	/// <returns>Returns the number of bytes written or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function does formatted printing to the stream.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IOprintf">SDL_IOprintf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial nuint SDL_IOprintf(SDL_IOStream* context, byte* fmt);

	/// <seealso cref="SDL_IOprintf(SDL_IOStream*, byte*)"/>
	[NativeImportSymbol<Library>("SDL_IOprintf", Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly byte SDL_IOprintf_var();

	/// <summary>
	/// Load all the data from an SDL data stream
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to read all available data from</param>
	/// <param name="datasize">A pointer filled in with the number of bytes read, may be NULL</param>
	/// <param name="closeio">if true, calls <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>() on src before returning, even in the case of an error</param>
	/// <returns>Returns the data or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// The data is allocated with a zero byte at the end (null terminated) for convenience. This extra byte is not included in the value reported via <c><paramref name="datasize"/></c>.
	/// </para>
	/// <para>
	/// The data should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadFile_IO">SDL_LoadFile_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_LoadFile_IO(SDL_IOStream* src, nuint* datasize, CBool closeio);

	/// <summary>
	/// Create a custom <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="iface">The interface that implements this <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>, initialized using <see href="https://wiki.libsdl.org/SDL3/SDL_INIT_INTERFACE">SDL_INIT_INTERFACE</see>()</param>
	/// <param name="userdata">The pointer that will be passed to the interface functions</param>
	/// <returns>Returns a pointer to the allocated memory on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Applications do not need to use this function unless they are providing their own <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> implementation.
	/// If you just need an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to read/write a common data source, you should use the built-in implementations in SDL,
	/// like <see href="https://wiki.libsdl.org/SDL3/SDL_IOFromFile">SDL_IOFromFile</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_IOFromMem">SDL_IOFromMem</see>(), etc.
	/// </para>
	/// <para>
	/// This function makes a copy of <c><paramref name="iface"/></c> and the caller does not need to keep it around after this call.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OpenIO">SDL_OpenIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_IOStream* SDL_OpenIO(SDL_IOStreamInterface* iface, void* userdata);

	/// <summary>
	/// Read from a data source
	/// </summary>
	/// <param name="context">A pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure</param>
	/// <param name="ptr">A pointer to a buffer to read data into</param>
	/// <param name="size">The number of bytes to read from the data source</param>
	/// <returns>Returns the number of bytes read, or 0 on end of file or other failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function reads up <c><paramref name="size"/></c> bytes from the data source to the area pointed at by <c><paramref name="ptr"/></c>. This function may read less bytes than requested.
	/// </para>
	/// <para>
	/// This function will return zero when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If zero is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadIO">SDL_ReadIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial nuint SDL_ReadIO(SDL_IOStream* context, void* ptr, nuint size);

	/// <summary>
	/// Use this function to read 16 bits of big-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS16BE">SDL_ReadS16BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS16BE(SDL_IOStream* src, short* value);

	/// <summary>
	/// Use this function to read 16 bits of little-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS16LE">SDL_ReadS16LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS16LE(SDL_IOStream* src, short* value);

	/// <summary>
	/// Use this function to read 32 bits of big-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS32BE">SDL_ReadS32BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS32BE(SDL_IOStream* src, int* value);

	/// <summary>
	/// Use this function to read 32 bits of little-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS32LE">SDL_ReadS32LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS32LE(SDL_IOStream* src, int* value);

	/// <summary>
	/// Use this function to read 64 bits of big-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS64BE">SDL_ReadS64BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS64BE(SDL_IOStream* src, long* value);

	/// <summary>
	/// Use this function to read 64 bits of little-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS64LE">SDL_ReadS64LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS64LE(SDL_IOStream* src, long* value);

	/// <summary>
	/// Use this function to read a signed byte from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to read from</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadS8">SDL_ReadS8</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadS8(SDL_IOStream* src, sbyte* value);

	/// <summary>
	/// Use this function to read 16 bits of big-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU16BE">SDL_ReadU16BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU16BE(SDL_IOStream* src, ushort* value);

	/// <summary>
	/// Use this function to read 16 bits of little-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU16LE">SDL_ReadU16LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU16LE(SDL_IOStream* src, ushort* value);

	/// <summary>
	/// Use this function to read 32 bits of big-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU32BE">SDL_ReadU32BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU32BE(SDL_IOStream* src, uint* value);

	/// <summary>
	/// Use this function to read 32 bits of little-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU32LE">SDL_ReadU32LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU32LE(SDL_IOStream* src, uint* value);

	/// <summary>
	/// Use this function to read 64 bits of big-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU64BE">SDL_ReadU64BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU64BE(SDL_IOStream* src, ulong* value);

	/// <summary>
	/// Use this function to read 64 bits of little-endian data from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> and return in native format
	/// </summary>
	/// <param name="src">The stream from which to read data</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the data returned will be in the native byte order.
	/// </para>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU64LE">SDL_ReadU64LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU64LE(SDL_IOStream* src, ulong* value);

	/// <summary>
	/// Use this function to read a byte from an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to read from</param>
	/// <param name="value">A pointer filled in with the data read</param>
	/// <returns>Returns true on successful read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function will return false when the data stream is completely read, and <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return <see href="https://wiki.libsdl.org/SDL3/SDL_IO_STATUS_EOF">SDL_IO_STATUS_EOF</see>.
	/// If false is returned and the stream is not at EOF, <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() will return a different error value and <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() will offer a human-readable message.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadU8">SDL_ReadU8</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadU8(SDL_IOStream* src, byte* value);

	/// <summary>
	/// Save all the data into an SDL data stream
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to write all data to</param>
	/// <param name="data">The data to be written. If datasize is 0, may be NULL or a invalid pointer</param>
	/// <param name="datasize">The number of bytes to be written</param>
	/// <param name="closeio">If true, calls <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>() on src before returning, even in the case of an error</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SaveFile_IO">SDL_SaveFile_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SaveFile_IO(SDL_IOStream* src, void* data, nuint datasize, CBool closeio);

	/// <summary>
	/// Seek within <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">an SDL_IOStream</see> data stream
	/// </summary>
	/// <param name="context">A pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure</param>
	/// <param name="offset">An offset in bytes, relative to whence location; can be negative</param>
	/// <param name="whence">Any of <see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_SET">SDL_IO_SEEK_SET</see>, <see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_CUR">SDL_IO_SEEK_CUR</see>, <see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_END">SDL_IO_SEEK_END</see></param>
	/// <returns>Returns the final offset in the data stream after the seek or -1 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function seeks to byte <c><paramref name="offset"/></c>, relative to <c><paramref name="whence"/></c>.
	/// </para>
	/// <para>
	/// <c><paramref name="whence"/></c> may be any of the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_SET"><c>SDL_IO_SEEK_SET</c></see></term>
	///			<description>Seek from the beginning of data</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_CUR"><c>SDL_IO_SEEK_CUR</c></see></term>
	///			<description>Seek relative to current read point</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_END"><c>SDL_IO_SEEK_END</c></see></term>
	///			<description>Seek relative to the end of data</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// If this stream can not seek, it will return -1.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SeekIO">SDL_SeekIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial long SDL_SeekIO(SDL_IOStream* context, long offset, StreamWhence whence);

	/// <summary>
	/// Determine the current read/write offset in an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> data stream
	/// </summary>
	/// <param name="context">An <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> data stream object from which to get the current offset</param>
	/// <returns>Returns the current offset in the stream, or -1 if the information can not be determined</returns>
	/// <remarks>
	/// <para>
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_TellIO">SDL_TellIO</see> is actually a wrapper function that calls the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>'s seek method,
	/// with an offset of 0 bytes from <see href="https://wiki.libsdl.org/SDL3/SDL_IO_SEEK_CUR">SDL_IO_SEEK_CUR</see>, to simplify application development.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks> 
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TellIO">SDL_TellIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial long SDL_TellIO(SDL_IOStream* context);

	/// <summary>
	/// Write to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> data stream
	/// </summary>
	/// <param name="context">A pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure</param>
	/// <param name="ptr">A pointer to a buffer containing data to write</param>
	/// <param name="size">The number of bytes to write</param>
	/// <returns>Returns the number of bytes written, which will be less than <c><paramref name="size"/></c> on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function writes exactly <c><paramref name="size"/></c> bytes from the area pointed at by <c><paramref name="ptr"/></c> to the stream.
	/// If this fails for any reason, it'll return less than <c><paramref name="size"/></c> to demonstrate how far the write progressed.
	/// On success, it returns <c><paramref name="size"/></c>.
	/// </para>
	/// <para>
	/// On error, this function still attempts to write as much as possible, so it might return a positive value less than the requested write size.
	/// </para>
	/// <para>
	/// The caller can use <see href="https://wiki.libsdl.org/SDL3/SDL_GetIOStatus">SDL_GetIOStatus</see>() to determine if the problem is recoverable, such as a non-blocking write that can simply be retried later, or a fatal error.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteIO">SDL_WriteIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial nuint SDL_WriteIO(SDL_IOStream* context, void* ptr, nuint size);

	/// <summary>
	/// Use this function to write 16 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as big-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in big-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS16BE">SDL_WriteS16BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS16BE(SDL_IOStream* dst, short value);

	/// <summary>
	/// Use this function to write 16 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as little-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in little-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS16LE">SDL_WriteS16LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS16LE(SDL_IOStream* dst, short value);

	/// <summary>
	/// Use this function to write 32 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as big-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in big-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS32BE">SDL_WriteS32BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS32BE(SDL_IOStream* dst, int value);

	/// <summary>
	/// Use this function to write 32 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as little-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in little-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS32LE">SDL_WriteS32LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS32LE(SDL_IOStream* dst, int value);

	/// <summary>
	/// Use this function to write 64 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as big-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in big-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS64BE">SDL_WriteS64BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS64BE(SDL_IOStream* dst, long value);

	/// <summary>
	/// Use this function to write 64 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as little-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in little-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS64LE">SDL_WriteS64LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS64LE(SDL_IOStream* dst, long value);

	/// <summary>
	/// Use this function to write a signed byte to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to write to</param>
	/// <param name="value">The byte value to write</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteS8">SDL_WriteS8</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteS8(SDL_IOStream* dst, sbyte value);

	/// <summary>
	/// Use this function to write 16 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as big-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in big-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU16BE">SDL_WriteU16BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU16BE(SDL_IOStream* dst, ushort value);

	/// <summary>
	/// Use this function to write 16 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as little-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in little-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU16LE">SDL_WriteU16LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU16LE(SDL_IOStream* dst, ushort value);

	/// <summary>
	/// Use this function to write 32 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as big-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in big-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU32BE">SDL_WriteU32BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU32BE(SDL_IOStream* dst, uint value);

	/// <summary>
	/// Use this function to write 32 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as little-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in little-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU32LE">SDL_WriteU32LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU32LE(SDL_IOStream* dst, uint value);

	/// <summary>
	/// Use this function to write 64 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as big-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in big-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU64BE">SDL_WriteU64BE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU64BE(SDL_IOStream* dst, ulong value);

	/// <summary>
	/// Use this function to write 64 bits in native format to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> as little-endian data
	/// </summary>
	/// <param name="dst">The stream to which data will be written</param>
	/// <param name="value">The data to be written, in native format</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL byteswaps the data only if necessary, so the application always specifies native format, and the data written will be in little-endian format.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU64LE">SDL_WriteU64LE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU64LE(SDL_IOStream* dst, ulong value);

	/// <summary>
	/// Use this function to write a byte to an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> to write to</param>
	/// <param name="value">The byte value to write</param>
	/// <returns>Returns true on successful write or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteU8">SDL_WriteU8</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteU8(SDL_IOStream* dst, byte value);
}
