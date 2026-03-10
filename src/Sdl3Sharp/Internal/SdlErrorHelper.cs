using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Internal;

internal static class SdlErrorHelper
{
	internal const string ParameterInvalidErrorFormat = "Parameter '{0}' is invalid";

	/// <exception cref="SdlException">
	/// There was a <see cref="Error">SDL error</see> to throw
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static void ThrowIfFailed()
	{
		unsafe
		{
			if (Error.SDL_GetError() is var message
				&& message is not null)
			{
				fail(message);
			}

			[DoesNotReturn]
			static void fail(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

	/// <exception cref="SdlException">
	/// <paramref name="condition"/> is <c><see langword="false"/></c>
	/// and there was a <see cref="Error">SDL error</see> to throw
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static void ThrowIfFailed(bool condition)
	{
		if (!condition)
		{
			ThrowIfFailed();
		}
	}

	/// <exception cref="SdlException">
	/// There was a <see cref="Error">SDL error</see> to throw
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static void ThrowIfFailed(ReadOnlySpan<byte> filterError)
	{
		unsafe
		{
			if (Error.SDL_GetError() is var message
				&& message is not null
				&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual(filterError) /* filter out errors */)
			{
				fail(message);
			}

			[DoesNotReturn]
			static void fail(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}


	/// <exception cref="SdlException">
	/// <paramref name="condition"/> is <c><see langword="false"/></c>,
	/// there was a <see cref="Error">SDL error</see> to throw,
	/// and that error did not match <paramref name="filterError"/>
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static void ThrowIfFailed(bool condition, ReadOnlySpan<byte> filterError)
	{
		if (!condition)
		{
			ThrowIfFailed(filterError);
		}
	}
}
