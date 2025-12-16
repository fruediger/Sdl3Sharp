using Sdl3Sharp.Ffi;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Internal;

internal static class Variadic
{
	internal static void Invoke(ref readonly byte functionRef, int fixedArguments, Span<object> args)
	{
		unsafe
		{
			var utf8Strings = stackalloc byte*[args.Length];
			var utf8String = utf8Strings;

			try
			{
				foreach (ref var arg in args)
				{				
					if (arg is string stringArg)
					{
						var stringArgUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(stringArg);

						*utf8String++ = stringArgUtf8;
						arg = unchecked((IntPtr)stringArgUtf8);
					}
				}

				Ffi.Ffi.InvokeVariadic(Abi.Default, unchecked((IntPtr)Unsafe.AsPointer(ref Unsafe.AsRef(in functionRef))), fixedArguments, args);
			}
			finally
			{
				while (utf8String > utf8Strings)
				{
					Utf8StringMarshaller.Free(*--utf8String);
				}
			}
		}
	}

	internal static TResult Invoke<TResult>(ref readonly byte functionRef, int fixedArguments, Span<object> args)
		where TResult : unmanaged
	{
		unsafe
		{
			var utf8Strings = stackalloc byte*[args.Length];
			var utf8String = utf8Strings;

			try
			{
				foreach (ref var arg in args)
				{				
					if (arg is string stringArg)
					{
						var stringArgUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(stringArg);

						*utf8String++ = stringArgUtf8;
						arg = unchecked((IntPtr)stringArgUtf8);
					}
				}

				Ffi.Ffi.InvokeVariadic(Abi.Default, unchecked((IntPtr)Unsafe.AsPointer(ref Unsafe.AsRef(in functionRef))), fixedArguments, out TResult result, args);

				return result;
			}
			finally
			{
				while (utf8String > utf8Strings)
				{
					Utf8StringMarshaller.Free(*--utf8String);
				}
			}
		}
	}
}
