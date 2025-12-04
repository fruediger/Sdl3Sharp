using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

/// <summary>
/// Represents the lifetime of a set of initialized <see cref="Sdl3Sharp.SubSystems">sub systems</see>
/// </summary>
/// <remarks>
/// Initialized <see cref="Sdl3Sharp.SubSystems"/> are reference counted through <see cref="SubSystemInit"/>s.
/// That means the lifetime of a certain sub sets of <see cref="Sdl3Sharp.SubSystems"/> could overlap with other <see cref="SubSystemInit"/>s.
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed partial class SubSystemInit :
	IDisposable, Sdl.IDisposeReceiver, IFormattable, ISpanFormattable
{
	private static SimpleSpinYieldLock mLock = new();

	private WeakReference<Sdl>? mSdlReference;
	private SubSystems mSubSystems;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	/// <exception cref="SdlException">Couldn't initialize the specified sub systems (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	/// <exception cref="InvalidOperationException">Could not register the <see cref="SubSystemInit"/> with the <see cref="Sdl"/> instance</exception>
	internal SubSystemInit(Sdl sdl, SubSystems subSystems)
	{
		if (sdl is null)
		{
			failSdlArgumentNull();
		}

		// If 'subSystems' is empty, we don't fail;
		// instead we create an uninitialized default instance, which kind of behaves as if it was already disposed
		if (subSystems is not SubSystems.None)
		{
			// since we're going to check the currently initialized sub systems, before and after initializing the new ones,
			// we need to lock, as other threads might be initializing sub systems at the same time
			mLock.Enter(0);
			try
			{
				var before = sdl.GetInitializedSubSystems();

				if (!SDL_InitSubSystem(subSystems))
				{
					failCouldNotInitializeSubSystems();
				}

				subSystems |= sdl.GetInitializedSubSystems() & ~before;
			}
			finally
			{
				mLock.Exit(0);
			}
		}

		if (!sdl.TryRegisterDisposable(this))
		{
			if (subSystems is not Sdl3Sharp.SubSystems.None)
			{
				SDL_QuitSubSystem(subSystems);
			}

			failCouldNotRegisterWithSdl();
		}

		mSdlReference = new(sdl);
		mSubSystems = subSystems;

		[DoesNotReturn]
		static void failSdlArgumentNull() => throw new ArgumentNullException(nameof(sdl));

		[DoesNotReturn]
		static void failCouldNotInitializeSubSystems() => throw new SdlException("SDL could not initialize the specified sub systems");

		[DoesNotReturn]
		static void failCouldNotRegisterWithSdl() => throw new InvalidOperationException($"Couldn't register the {nameof(SubSystemInit)} with the {nameof(Sdl)} instance");
	}

	/// <inheritdoc/>
	~SubSystemInit() => Dispose(deregister: true);

	/// <summary>
	/// Gets the sub systems the <see cref="SubSystemInit"/> was initialized with
	/// </summary>
	/// <value>
	/// The sub systems the <see cref="SubSystemInit"/> was initialized with
	/// </value>
	public SubSystems SubSystems { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSubSystems; }

	/// <summary>
	/// Disposes the <see cref="SubSystemInit"/> and potentially shuts down some of the <see cref="Sdl3Sharp.SubSystems">sub systems</see>
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="Sdl3Sharp.SubSystems"/> are reference counted through <see cref="SubSystemInit"/>s.
	/// This method decreases the reference count for the <see cref="Sdl3Sharp.SubSystems"/>.
	/// </para>
	/// <para>
	/// <see cref="Sdl3Sharp.SubSystems"/> get shut down if their reference count reaches 0.
	/// That means if other <see cref="SubSystemInit"/>s hold a reference to certain <see cref="Sdl3Sharp.SubSystems"/> or the <see cref="Sdl"/> instance was initialized with certain <see cref="Sdl3Sharp.SubSystems"/>,
	/// those might not get shut down immediately.
	/// </para>
	/// <para>
	/// <see cref="Sdl.Dispose"/> shuts down all <see cref="Sdl3Sharp.SubSystems"/> regardless.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(deregister: true);
	}

	void Sdl.IDisposeReceiver.DisposeFromSdl(Sdl sdl)
	{
#pragma warning disable IDE0079
#pragma warning disable CA1816
		GC.SuppressFinalize(this);
#pragma warning restore CA1816
#pragma warning restore IDE0079
		// We don't need to deregister, since Sdl is going to be disposed anyways
		Dispose(deregister: false);
	}

	private void Dispose(bool deregister)
	{
		if (mSubSystems is not SubSystems.None)
		{
			if (mSdlReference is not null)
			{
				if (deregister && mSdlReference.TryGetTarget(out var sdl))
				{
					sdl.TryDeregisterDisposable(this);
				}

				mSdlReference = null;

				SDL_QuitSubSystem(mSubSystems);
			}

			mSubSystems = SubSystems.None;
		}
	}

	/// <inheritdoc/>
	public override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public string ToString(string? format, IFormatProvider? formatProvider) => $"{{ {nameof(SubSystems)}: [{mSubSystems.ToString(format)}] }}";

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(SubSystems)}: [", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mSubSystems, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite("] }", ref destination, ref charsWritten);
	}
}
