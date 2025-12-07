using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents a general event in SDL
/// </summary>
/// <remarks>
/// <para>
/// Even though SDL's *Event structures do not use inheritance (as they are all <c><see langword="struct"/></c>s or <see cref="ValueType"/>s),
/// you may think of <see cref="Event"/> as a common base <em>representation</em> of all the other *Event structures.
/// </para>
/// <para>
/// More specifically: all of the other *Event structures <em>can</em> be represented as an <see cref="Event"/>
/// (this is usually achieved through <see cref="ICommonEvent{TSelf}.implicit operator Event(in TSelf)"/>),
/// while an <see cref="Event"/> structure <em>could</em> potentially be represented as one of the *Event structures, depending on it's <see cref="Type"/>
/// (this can be achieved through <see cref="ICommonEvent{TSelf}.explicit operator TSelf(in Event)"/>,
/// or copyless through <see cref="EventExtensions.TryAs{TEvent}(ref Event, out Sdl3Sharp.Utilities.NullableRef{TEvent})"/> or <see cref="EventExtensions.TryAsReadOnly{TEvent}(ref readonly Event, out Sdl3Sharp.Utilities.NullableRefReadOnly{TEvent})"/>)
/// </para>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.Terminating"/></description></item>
/// <item><description><see cref="EventType.LowMemory"/></description></item>
/// <item><description><see cref="EventType.WillEnterBackground"/></description></item>
/// <item><description><see cref="EventType.DidEnterBackground"/></description></item>
/// <item><description><see cref="EventType.WillEnterForeground"/></description></item>
/// <item><description><see cref="EventType.DidEnterForeground"/></description></item>
/// <item><description><see cref="EventType.LocaleChanged"/></description></item>
/// <item><description><see cref="EventType.SystemThemeChanged"/></description></item>
/// <item><description><see cref="EventType.KeymapChanged"/></description></item>
/// <item><description><see cref="EventType.ScreenKeyboardShown"/></description></item>
/// <item><description><see cref="EventType.ScreenKeyboardHidden"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Explicit)]
public partial struct Event : ICommonEvent<Event>, IFormattable, ISpanFormattable
{
	internal interface IUnsafeConstructorDispatch;

	[InlineArray(128)] private struct Padding { private byte _; }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<Event>.Accepts(EventType type) => true; // Event accepts all EventTypes

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref Event ICommonEvent<Event>.GetReference(ref Event @event) => ref @event;

	[FieldOffset(0)] private readonly Padding mPadding;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal Event(IUnsafeConstructorDispatch? _ = default) => Unsafe.SkipInit(out this);

	/// <remarks>
	/// <para>
	/// Do not attempt to set this property on a base <see cref="Event"/>, instead set the <see cref="ICommonEvent.Type"/> property on an instance of a more specialized *Event structure.
	/// Setting this property on a base <see cref="Event"/> is not supported and will lead the property to throw a <see cref="NotSupportedException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="NotSupportedException">When setting this property</exception>
	/// <inheritdoc/>
	public EventType Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Common.Type;

		[Obsolete($"Setting the {nameof(Type)} of a base {nameof(Event)} is not supported.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting the {nameof(Type)} of a base {nameof(Event)} is not supported");
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Common.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => Common.Timestamp = value;
	}

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => 0 switch
	{
		_ when this.Is<QuitEvent>()                  => $"{nameof(QuitEvent)} {this.UnsafeAsReadOnly<QuitEvent>().ToString(format, formatProvider)}",
		_ when this.Is<DisplayEvent>()               => $"{nameof(DisplayEvent)} {this.UnsafeAsReadOnly<DisplayEvent>().ToString(format, formatProvider)}",
		_ when this.Is<WindowEvent>()                => $"{nameof(WindowEvent)} {this.UnsafeAsReadOnly<WindowEvent>().ToString(format, formatProvider)}",
		_ when this.Is<KeyboardEvent>()              => $"{nameof(KeyboardEvent)} {this.UnsafeAsReadOnly<KeyboardEvent>().ToString(format, formatProvider)}",
		_ when this.Is<TextEditingEvent>()           => $"{nameof(TextEditingEvent)} {this.UnsafeAsReadOnly<TextEditingEvent>().ToString(format, formatProvider)}",
		_ when this.Is<TextInputEvent>()             => $"{nameof(TextInputEvent)} {this.UnsafeAsReadOnly<TextInputEvent>().ToString(format, formatProvider)}",
		_ when this.Is<KeyboardDeviceEvent>()        => $"{nameof(KeyboardDeviceEvent)} {this.UnsafeAsReadOnly<KeyboardDeviceEvent>().ToString(format, formatProvider)}",
		_ when this.Is<TextEditingCandidatesEvent>() => $"{nameof(TextEditingCandidatesEvent)} {this.UnsafeAsReadOnly<TextEditingCandidatesEvent>().ToString(format, formatProvider)}",
		_ when this.Is<MouseMotionEvent>()           => $"{nameof(MouseMotionEvent)} {this.UnsafeAsReadOnly<MouseMotionEvent>().ToString(format, formatProvider)}",
		_ when this.Is<MouseButtonEvent>()           => $"{nameof(MouseButtonEvent)} {this.UnsafeAsReadOnly<MouseButtonEvent>().ToString(format, formatProvider)}",
		_ when this.Is<MouseWheelEvent>()            => $"{nameof(MouseWheelEvent)} {this.UnsafeAsReadOnly<MouseWheelEvent>().ToString(format, formatProvider)}",
		_ when this.Is<MouseDeviceEvent>()           => $"{nameof(MouseDeviceEvent)} {this.UnsafeAsReadOnly<MouseDeviceEvent>().ToString(format, formatProvider)}",
		_ when this.Is<JoyAxisEvent>()               => $"{nameof(JoyAxisEvent)} {this.UnsafeAsReadOnly<JoyAxisEvent>().ToString(format, formatProvider)}",
		_ when this.Is<JoyBallEvent>()               => $"{nameof(JoyBallEvent)} {this.UnsafeAsReadOnly<JoyBallEvent>().ToString(format, formatProvider)}",
		_ when this.Is<JoyHatEvent>()                => $"{nameof(JoyHatEvent)} {this.UnsafeAsReadOnly<JoyHatEvent>().ToString(format, formatProvider)}",
		_ when this.Is<JoyButtonEvent>()             => $"{nameof(JoyButtonEvent)} {this.UnsafeAsReadOnly<JoyButtonEvent>().ToString(format, formatProvider)}",
		_ when this.Is<JoyDeviceEvent>()             => $"{nameof(JoyDeviceEvent)} {this.UnsafeAsReadOnly<JoyDeviceEvent>().ToString(format, formatProvider)}",
		_ when this.Is<JoyBatteryEvent>()            => $"{nameof(JoyBatteryEvent)} {this.UnsafeAsReadOnly<JoyBatteryEvent>().ToString(format, formatProvider)}",
		_ when this.Is<GamepadAxisEvent>()           => $"{nameof(GamepadAxisEvent)} {this.UnsafeAsReadOnly<GamepadAxisEvent>().ToString(format, formatProvider)}",
		_ when this.Is<GamepadButtonEvent>()         => $"{nameof(GamepadButtonEvent)} {this.UnsafeAsReadOnly<GamepadButtonEvent>().ToString(format, formatProvider)}",
		_ when this.Is<GamepadDeviceEvent>()         => $"{nameof(GamepadDeviceEvent)} {this.UnsafeAsReadOnly<GamepadDeviceEvent>().ToString(format, formatProvider)}",
		_ when this.Is<GamepadTouchpadEvent>()       => $"{nameof(GamepadTouchpadEvent)} {this.UnsafeAsReadOnly<GamepadTouchpadEvent>().ToString(format, formatProvider)}",
		_ when this.Is<GamepadSensorEvent>()         => $"{nameof(GamepadSensorEvent)} {this.UnsafeAsReadOnly<GamepadSensorEvent>().ToString(format, formatProvider)}",
		_ when this.Is<TouchFingerEvent>()           => $"{nameof(TouchFingerEvent)} {this.UnsafeAsReadOnly<TouchFingerEvent>().ToString(format, formatProvider)}",
		_ when this.Is<PinchFingerEvent>()           => $"{nameof(PinchFingerEvent)} {this.UnsafeAsReadOnly<PinchFingerEvent>().ToString(format, formatProvider)}",
		_ when this.Is<ClipboardEvent>()             => $"{nameof(ClipboardEvent)} {this.UnsafeAsReadOnly<ClipboardEvent>().ToString(format, formatProvider)}",
		_ when this.Is<DropEvent>()                  => $"{nameof(DropEvent)} {this.UnsafeAsReadOnly<DropEvent>().ToString(format, formatProvider)}",
		_ when this.Is<AudioDeviceEvent>()           => $"{nameof(AudioDeviceEvent)} {this.UnsafeAsReadOnly<AudioDeviceEvent>().ToString(format, formatProvider)}",
		_ when this.Is<SensorEvent>()                => $"{nameof(SensorEvent)} {this.UnsafeAsReadOnly<SensorEvent>().ToString(format, formatProvider)}",
		_ when this.Is<PenProximityEvent>()          => $"{nameof(PenProximityEvent)} {this.UnsafeAsReadOnly<PenProximityEvent>().ToString(format, formatProvider)}",
		_ when this.Is<PenTouchEvent>()              => $"{nameof(PenTouchEvent)} {this.UnsafeAsReadOnly<PenTouchEvent>().ToString(format, formatProvider)}",
		_ when this.Is<PenButtonEvent>()             => $"{nameof(PenButtonEvent)} {this.UnsafeAsReadOnly<PenButtonEvent>().ToString(format, formatProvider)}",
		_ when this.Is<PenMotionEvent>()             => $"{nameof(PenMotionEvent)} {this.UnsafeAsReadOnly<PenMotionEvent>().ToString(format, formatProvider)}",
		_ when this.Is<PenAxisEvent>()               => $"{nameof(PenAxisEvent)} {this.UnsafeAsReadOnly<PenAxisEvent>().ToString(format, formatProvider)}",
		_ when this.Is<CameraDeviceEvent>()          => $"{nameof(CameraDeviceEvent)} {this.UnsafeAsReadOnly<CameraDeviceEvent>().ToString(format, formatProvider)}",
		_ when this.Is<RenderEvent>()                => $"{nameof(RenderEvent)} {this.UnsafeAsReadOnly<RenderEvent>().ToString(format, formatProvider)}",
		_ when this.Is<UserEvent>()                  => $"{nameof(UserEvent)} {this.UnsafeAsReadOnly<UserEvent>().ToString(format, formatProvider)}",
		_                                            => $"{nameof(Event)} {this.UnsafeAsReadOnly<CommonEvent>().ToString(format, formatProvider)}"
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default) => (charsWritten = 0) switch
	{
		_ when this.Is<QuitEvent>()                  => SpanFormat.TryWrite($"{nameof(QuitEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<QuitEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<DisplayEvent>()               => SpanFormat.TryWrite($"{nameof(DisplayEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<DisplayEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<WindowEvent>()                => SpanFormat.TryWrite($"{nameof(WindowEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<WindowEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<KeyboardEvent>()              => SpanFormat.TryWrite($"{nameof(KeyboardEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<KeyboardEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<TextEditingEvent>()           => SpanFormat.TryWrite($"{nameof(TextEditingEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<TextEditingEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<TextInputEvent>()             => SpanFormat.TryWrite($"{nameof(TextInputEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<TextInputEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<KeyboardDeviceEvent>()        => SpanFormat.TryWrite($"{nameof(KeyboardDeviceEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<KeyboardDeviceEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<TextEditingCandidatesEvent>() => SpanFormat.TryWrite($"{nameof(TextEditingCandidatesEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<TextEditingCandidatesEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<MouseMotionEvent>()           => SpanFormat.TryWrite($"{nameof(MouseMotionEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<MouseMotionEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<MouseButtonEvent>()           => SpanFormat.TryWrite($"{nameof(MouseButtonEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<MouseButtonEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<MouseWheelEvent>()            => SpanFormat.TryWrite($"{nameof(MouseWheelEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<MouseWheelEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<MouseDeviceEvent>()           => SpanFormat.TryWrite($"{nameof(MouseDeviceEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<MouseDeviceEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<JoyAxisEvent>()               => SpanFormat.TryWrite($"{nameof(JoyAxisEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<JoyAxisEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<JoyBallEvent>()               => SpanFormat.TryWrite($"{nameof(JoyBallEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<JoyBallEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<JoyHatEvent>()                => SpanFormat.TryWrite($"{nameof(JoyHatEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<JoyHatEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<JoyButtonEvent>()             => SpanFormat.TryWrite($"{nameof(JoyButtonEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<JoyButtonEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<JoyDeviceEvent>()             => SpanFormat.TryWrite($"{nameof(JoyDeviceEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<JoyDeviceEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<JoyBatteryEvent>()            => SpanFormat.TryWrite($"{nameof(JoyBatteryEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<JoyBatteryEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<GamepadAxisEvent>()           => SpanFormat.TryWrite($"{nameof(GamepadAxisEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<GamepadAxisEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<GamepadButtonEvent>()         => SpanFormat.TryWrite($"{nameof(GamepadButtonEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<GamepadButtonEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<GamepadDeviceEvent>()         => SpanFormat.TryWrite($"{nameof(GamepadDeviceEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<GamepadDeviceEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<GamepadTouchpadEvent>()       => SpanFormat.TryWrite($"{nameof(GamepadTouchpadEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<GamepadTouchpadEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<GamepadSensorEvent>()         => SpanFormat.TryWrite($"{nameof(GamepadSensorEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<GamepadSensorEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<TouchFingerEvent>()           => SpanFormat.TryWrite($"{nameof(TouchFingerEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<TouchFingerEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<PinchFingerEvent>()           => SpanFormat.TryWrite($"{nameof(PinchFingerEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<PinchFingerEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<ClipboardEvent>()             => SpanFormat.TryWrite($"{nameof(ClipboardEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<ClipboardEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<DropEvent>()                  => SpanFormat.TryWrite($"{nameof(DropEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<DropEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<AudioDeviceEvent>()           => SpanFormat.TryWrite($"{nameof(AudioDeviceEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<AudioDeviceEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<SensorEvent>()                => SpanFormat.TryWrite($"{nameof(SensorEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<SensorEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<PenProximityEvent>()          => SpanFormat.TryWrite($"{nameof(PenProximityEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<PenProximityEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<PenTouchEvent>()              => SpanFormat.TryWrite($"{nameof(PenTouchEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<PenTouchEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<PenButtonEvent>()             => SpanFormat.TryWrite($"{nameof(PenButtonEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<PenButtonEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<PenMotionEvent>()             => SpanFormat.TryWrite($"{nameof(PenMotionEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<PenMotionEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<PenAxisEvent>()               => SpanFormat.TryWrite($"{nameof(PenAxisEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<PenAxisEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<CameraDeviceEvent>()          => SpanFormat.TryWrite($"{nameof(CameraDeviceEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<CameraDeviceEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<RenderEvent>()                => SpanFormat.TryWrite($"{nameof(RenderEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<RenderEvent>(), ref destination, ref charsWritten, format, provider),
		_ when this.Is<UserEvent>()                  => SpanFormat.TryWrite($"{nameof(UserEvent)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<UserEvent>(), ref destination, ref charsWritten, format, provider),
		_                                            => SpanFormat.TryWrite($"{nameof(Event)} ", ref destination, ref charsWritten)
		                                             && SpanFormat.TryWrite(in this.UnsafeAsReadOnly<CommonEvent>(), ref destination, ref charsWritten, format, provider)
	};

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static implicit ICommonEvent<Event>.operator Event(in Event @event) => @event;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static explicit ICommonEvent<Event>.operator Event(in Event @event) => @event;
}
