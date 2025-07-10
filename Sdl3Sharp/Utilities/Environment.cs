using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a set of environment variables
/// </summary>
/// <remarks>
/// <para>
/// Operations on instances of this class are thread-safe, except when using the <see cref="TryGetProcessVariableUnsafe(string, out string?)"/>, <see cref="TrySetProcessVariableUnsafe(string, string, bool)"/>, and <see cref="TryUnsetProcessVariableUnsafe(string)"/> methods."/>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed partial class Environment : IDisposable, IEnumerable<KeyValuePair<string, string>>, IEquatable<Environment>
{
	private SdlDisposeReceiver? mSdlDisposeReceiver;
	private unsafe SDL_Environment* mEnvironmentPtr;

	//TODO: fix this
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private unsafe string DebuggerDisplay => mEnvironmentPtr is not null
		? string.Join(" ", this.Select(p => $"{p.Key}={p.Value}"))
		: "<Invalid>";

	//private Environment(Implementation implementation) => mImplementation = implementation;

	/// <summary>
	/// Creates a new <see cref="Environment">set of environment variables</see>
	/// </summary>
	/// <param name="populateFromRuntime">Indicates whether the newly created <see cref="Environment"/> should be initialized with the environment variables from the C runtime environment</param>
	/// <remarks>
	/// <para>
	/// If <paramref name="populateFromRuntime"/> is set to <c><see langword="false"/></c> (its default value), it is safe to call this constructor from any thread,
	/// otherwise it is only safe to call, if there are no other threads that are calling <see cref="TrySetProcessVariableUnsafe(string, string, bool)"/> or <see cref="TryUnsetProcessVariableUnsafe(string)"/>.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this constructor intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this constructor in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't create a new <see cref="Environment"/></exception>
	public Environment(bool populateFromRuntime = false)
	{
		unsafe
		{
			if (!(SDL_CreateEnvironment(populateFromRuntime) is var environmentPtr && environmentPtr is not null))
			{
				failCouldNotCreateEnvironment();
			}

			mSdlDisposeReceiver = null;
			mEnvironmentPtr = environmentPtr;

			static void failCouldNotCreateEnvironment() => throw new SdlException($"Could not create a new {nameof(Environment)}");
		}
	}

	/// <exception cref="InvalidOperationException">Could not register the <see cref="Environment"/> with the given <paramref name="sdl"/> instance</exception>
	internal unsafe Environment(Sdl sdl, SDL_Environment* environmentPtr)
	{
		if (sdl is not null)
		{
			var sdlDisposeReceiver = new SdlDisposeReceiver(sdl, this);

			if (!sdl.TryRegisterDisposable(sdlDisposeReceiver))
			{
				failCouldNotRegisterWithSdl();
			}

			mSdlDisposeReceiver = sdlDisposeReceiver;
		}

		mEnvironmentPtr = environmentPtr;

		[DoesNotReturn]
		static void failCouldNotRegisterWithSdl() => throw new InvalidOperationException($"Couldn't register the {nameof(Environment)} with the given {nameof(Sdl)} instance");
	}

	/// <inheritdoc/>
	~Environment() => Dispose(deregister: true);

	/// <inheritdoc/>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(deregister: true);
	}

	private void DisposeFromSdl()
	{		
#pragma warning disable IDE0079
#pragma warning disable CA1816
		GC.SuppressFinalize(this);
#pragma warning restore CA1816
#pragma warning restore IDE0079
		Dispose(deregister: false);
	}

	private unsafe void Dispose(bool deregister)
	{
		if (mEnvironmentPtr is not null)
		{
			if (mSdlDisposeReceiver is not null)
			{
				if (deregister && mSdlDisposeReceiver.Sdl is { } sdl)
				{
					sdl.TryDeregisterDisposable(mSdlDisposeReceiver);
				}
				
				mSdlDisposeReceiver.Dispose();
				mSdlDisposeReceiver = null;
			}

			SDL_DestroyEnvironment(mEnvironmentPtr);

			mEnvironmentPtr = null;
		}
	}

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)]object? obj) => Equals(obj as Environment);

	/// <inheritdoc/>
	public bool Equals([NotNullWhen(true)] Environment? other)
	{
		unsafe
		{
			return other is { mEnvironmentPtr: var otherPtr } && mEnvironmentPtr == otherPtr;
		}
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		unsafe
		{
			return unchecked((IntPtr)mEnvironmentPtr).GetHashCode();
		}
	}

	/// <summary>
	/// Tries to enumerator the process environment variables
	/// </summary>
	/// <param name="enumerator">The resulting <see cref="Enumerator"/> to enumerate the process environment variables, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c> if an <see cref="Enumerator"/> for the process environment variables were successfully created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Use the resulting <paramref name="enumerator"/> to enumerate the process environment variables.
	/// </para>
	/// </remarks>
	public static bool TryGetProcessEnumerator([NotNullWhen(true)] out Enumerator? enumerator)
	{
		unsafe
		{
			if (SDL_GetEnvironment() is var environmentPtr && environmentPtr is not null
			 && SDL_GetEnvironmentVariables(environmentPtr) is var array && array is not null)
			{
				enumerator = new Enumerator(array);
				return true;
			}

			enumerator = null;
			return false;
		}
	}

	/// <summary>
	/// Tries to get the value of a process environment variable
	/// </summary>
	/// <param name="name">The name of the variable to get</param>
	/// <param name="value">The value of the environment variable, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="string"/>?)</c></param>
	/// <returns><c><see langword="true"/></c> if the process environment variable exists and its value could get retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method uses SDL's cached copy of the process environment and therefore is thread-safe.
	/// </para>
	/// <para>
	/// Alternatively to this method, you could use <see cref="TryGetVariable(string, out string?)"/> on <see cref="Sdl.ProcessEnvironment"/> instead.
	/// </para>
	/// </remarks>
	public static bool TryGetProcessVariable(string name, [NotNullWhen(true)] out string? value)
	{
		unsafe
		{
			var nameUtf8= Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				if (SDL_getenv(nameUtf8) is var valuePtr && valuePtr is not null)
				{
					value = Utf8StringMarshaller.ConvertToManaged(valuePtr);
					return value is not null;
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}

			value = default;
			return false;
		}
	}

	/// <summary>
	/// Tries to get the value of a process environment variable
	/// </summary>
	/// <param name="name">The name of the variable to get</param>
	/// <param name="value">The value of the environment variable, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="string"/>?)</c></param>
	/// <returns><c><see langword="true"/></c> if the process environment variable exists and its value could get retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method bypasses SDL's cached copy of the process environment and therefore is <em>not</em> thread-safe.
	/// </para>
	/// </remarks>
	public static bool TryGetProcessVariableUnsafe(string name, [NotNullWhen(true)] out string? value)
	{
		unsafe
		{
			var nameUtf8= Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				if (SDL_getenv_unsafe(nameUtf8) is var valuePtr && valuePtr is not null)
				{
					value = Utf8StringMarshaller.ConvertToManaged(valuePtr);
					return value is not null;
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}

			value = default;
			return false;
		}
	}

	/// <summary>
	/// Tries to set a process environment variable
	/// </summary>
	/// <param name="name">The name of the variable to set</param>
	/// <param name="value">The value of the environment variable to set to</param>
	/// <param name="overwrite">
	/// Indicates whether the value an existing environment variable should be overwritten.
	/// If set to <c><see langword="true"/></c>, the value of the environment variable will be set to the given <paramref name="value"/>, even if the variable already exists;
	/// otherwise, if set to <c><see langword="false"/></c>, an existing environment variable will not be changed while this method will still return successfully.
	/// </param>
	/// <returns><c><see langword="true"/></c> if the process environment variable was successfully set to <paramref name="value"/>, or if <paramref name="overwrite"/> was set to <c><see langword="false"/></c> and the environment variable already existed; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is <em>not</em> thread-safe, consider using <see cref="TrySetVariable(string, string, bool)"/> on <see cref="Sdl.ProcessEnvironment"/> instead.
	/// </para>
	/// </remarks>
	public static bool TrySetProcessVariableUnsafe(string name, string value, bool overwrite = true)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);
			var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);

			try
			{
				return SDL_setenv_unsafe(nameUtf8, valueUtf8, overwrite ? 1 : 0) is 0;
			}
			finally
			{
				Utf8StringMarshaller.Free(valueUtf8);
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to clear a process environment variable (remove it from the environment)
	/// </summary>
	/// <param name="name">The name of the variable to clear</param>
	/// <returns><c><see langword="true"/></c> if the process environment variable was successfully cleared; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is <em>not</em> thread-safe, consider using <see cref="TryUnsetVariable(string)"/> on <see cref="Sdl.ProcessEnvironment"/> instead.
	/// </para>
	/// </remarks>
	public static bool TryUnsetProcessVariableUnsafe(string name)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_unsetenv_unsafe(nameUtf8) is 0;
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to get the value of an environment variable in the current <see cref="Environment"/>
	/// </summary>
	/// <param name="name">The name of the variable to get</param>
	/// <param name="value">The value of the environment variable, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="string"/>?)</c></param>
	/// <returns><c><see langword="true"/></c> if the environment variable exists in the current <see cref="Environment"/> and its value could get retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryGetVariable(string name, [NotNullWhen(true)] out string? value)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				if (SDL_GetEnvironmentVariable(mEnvironmentPtr, nameUtf8) is var valuePtr && valuePtr is not null)
				{
					value = Utf8StringMarshaller.ConvertToManaged(valuePtr);
					return value is not null;
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}

			value = default;
			return false;
		}
	}

	/// <summary>
	/// Tries to set an environment variable in the current <see cref="Environment"/>
	/// </summary>
	/// <param name="name">The name of the variable to set</param>
	/// <param name="value">The value of the environment variable to set to</param>
	/// <param name="overwrite">
	/// Indicates whether the value an existing environment variable should be overwritten.
	/// If set to <c><see langword="true"/></c>, the value of the environment variable will be set to the given <paramref name="value"/>, even if the variable already exists;
	/// otherwise, if set to <c><see langword="false"/></c>, an existing environment variable will not be changed while this method will still return successfully.
	/// </param>
	/// <returns><c><see langword="true"/></c> if the environment variable in the current <see cref="Environment"/> was successfully set to <paramref name="value"/>, or if <paramref name="overwrite"/> was set to <c><see langword="false"/></c> and the environment variable already existed; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetVariable(string name, string value, bool overwrite = true)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);
			var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);

			try
			{
				return SDL_SetEnvironmentVariable(mEnvironmentPtr, nameUtf8, valueUtf8, overwrite);
			}
			finally
			{				
				Utf8StringMarshaller.Free(valueUtf8);
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to clear an environment variable in the current <see cref="Environment"/> (remove it from the environment)
	/// </summary>
	/// <param name="name">The name of the variable to clear</param>
	/// <returns><c><see langword="true"/></c> if the environment variable in the current <see cref="Environment"/> was successfully cleared; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryUnsetVariable(string name)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_UnsetEnvironmentVariable(mEnvironmentPtr, nameUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}
}
