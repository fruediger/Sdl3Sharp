using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Rendering;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Provides extension methods for <see cref="Event"/> and other *Event structures
/// </summary>
public static partial class EventExtensions
{
	extension<TEvent>(ref TEvent @event)
		where TEvent : unmanaged, ICommonEvent<TEvent>
	{
		/// <summary>
		/// Tries to convert the coordinates of an event to render coordinates, in place
		/// </summary>
		/// <typeparam name="TRenderer">The type of renderer to convert the event for</typeparam>
		/// <param name="renderer">The renderer to convert the event for</param>
		/// <returns><c><see langword="true"/></c>, if the conversion was successful and the event was modified in place; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method modifies the event in place. If you want to keep the original event, you should make a copy of it beforehand.
		/// </para>
		/// <para>
		/// The conversion takes into account several factors:
		/// <list type="bullet">
		///	<item><description>The <see cref="Renderer.Window"/> dimensions</description></item>
		///	<item><description>The <see cref="Renderer.LogicalPresentation"/> settings</description></item>
		///	<item><description>The <see cref="Renderer.Scale"/> settings</description></item>
		///	<item><description>The <see cref="Renderer.Viewport"/> settings</description></item>
		/// </list>
		/// </para>
		/// <para>
		/// Various event types can be converted with this method, including <see cref="MouseButtonEvent"/>, <see cref="MouseMotionEvent"/>, <see cref="MouseWheelEvent"/>, <see cref="TouchFingerEvent"/>, <see cref="PenMotionEvent"/>, <see cref="PenTouchEvent"/>, etc.
		/// </para>
		/// <para>
		/// Touch coordinates are converted from normalized coordinates in the window to non-normalized rendering coordinates.
		/// </para>
		/// <para>
		/// Relative mouse coordinates (e.g. <see cref="MouseMotionEvent.RelativeX"/> and <see cref="MouseMotionEvent.RelativeY"/>) are <em>also</em> converted.
		/// Applications that do not want these coordinates to be converted should use the <see cref="Renderer.TryConvertWindowToRenderCoordinates(float, float, out float, out float)"/> method
		/// with the specific property values of the event instead of converting the entire event.
		/// </para>
		/// <para>
		/// Converted coordinates may be outside of the bound of the current rendering area.
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryConvertToRenderCoordinates<TRenderer>(TRenderer renderer)
			where TRenderer : notnull, Renderer
		{
			unsafe
			{
				// we're going to do a dangerous pointer cast here, so we need to make sure the event is of the correct type first
				// if SDL was to read beyond the (memory) bounds of the event struct, just because it expected a different event type, we'd be in trouble

				if (!TEvent.Accepts(@event.Type))
				{
					return false;
				}

				fixed (TEvent* eventPtr = &@event)
				{
					return SDL_ConvertEventToRenderCoordinates(renderer is not null ? renderer.Pointer : null, unchecked((Event*)eventPtr));
				}
			}
		}
	}

	extension(ref Event @event)
	{
		/// <summary>
		/// Tries to reference an <see cref="Event"/> as a specific <typeparamref name="TEvent"/>
		/// </summary>
		/// <typeparam name="TEvent">The type of event to cast the <see cref="Event"/> to</typeparam>
		/// <param name="result">The <see cref="Event"/> referenced as a specific <typeparamref name="TEvent"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="NullableRef{T}"/>)</c></param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="Event"/> is a <typeparamref name="TEvent"/> by it's <see cref="Event.Type"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool TryAs<TEvent>(out NullableRef<TEvent> result)
			where TEvent : unmanaged, ICommonEvent<TEvent>
		{
			if (TEvent.Accepts(@event.Type))
			{
				result = new(ref TEvent.GetReference(ref @event));

				return true;
			}

			result = default;

			return false;
		}

		/// <summary>
		/// References an <see cref="Event"/> as a specific <typeparamref name="TEvent"/>
		/// </summary>
		/// <typeparam name="TEvent">The type of event to cast the <see cref="Event"/> to</typeparam>
		/// <returns>The <see cref="Event"/> referenced as a specific <typeparamref name="TEvent"/></returns>
		/// <remarks>
		/// <para>
		/// <em>WARNING</em>: Use with caution. This method does not perform any type checking. You should check the <see cref="Event"/>'s <see cref="Event.Type"/> manually before calling this method to ensure it is of the expected type!
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public ref TEvent UnsafeAs<TEvent>()
			where TEvent : unmanaged, ICommonEvent<TEvent>
			=> ref TEvent.GetReference(ref @event);
	}

	extension(ref readonly Event @event)
	{
		/// <summary>
		/// Determines whether the <see cref="Event"/> is a specific <typeparamref name="TEvent"/>
		/// </summary>
		/// <typeparam name="TEvent">The type of event to check against</typeparam>
		/// <returns><c><see langword="true"/></c>, if the <see cref="Event"/> is a <typeparamref name="TEvent"/> by it's <see cref="Event.Type"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool Is<TEvent>()
			where TEvent : unmanaged, ICommonEvent<TEvent>
			=> TEvent.Accepts(@event.Type);

		/// <summary>
		/// Tries to reference an <see cref="Event"/> as a specific <em>read-only</em> <typeparamref name="TEvent"/>
		/// </summary>
		/// <typeparam name="TEvent">The type of event to cast the <see cref="Event"/> to</typeparam>
		/// <param name="result">The <see cref="Event"/> referenced as a specific <em>read-only</em> <typeparamref name="TEvent"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="NullableRefReadOnly{T}"/>)</c></param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="Event"/> is a <typeparamref name="TEvent"/> by it's <see cref="Event.Type"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool TryAsReadOnly<TEvent>(out NullableRefReadOnly<TEvent> result)
			where TEvent : unmanaged, ICommonEvent<TEvent>
		{
			if (TEvent.Accepts(@event.Type))
			{
				result = new(ref TEvent.GetReference(ref Unsafe.AsRef(in @event)));

				return true;
			}

			result = default;

			return false;
		}

		/// <summary>
		/// References an <see cref="Event"/> as a specific <em>read-only</em> <typeparamref name="TEvent"/>
		/// </summary>
		/// <typeparam name="TEvent">The type of event to cast the <see cref="Event"/> to</typeparam>
		/// <returns>The <see cref="Event"/> referenced as a specific <em>read-only</em> <typeparamref name="TEvent"/></returns>
		/// <remarks>
		/// <para>
		/// <em>WARNING</em>: Use with caution. This method does not perform any type checking. You should check the <see cref="Event"/>'s <see cref="Event.Type"/> manually before calling this method to ensure it is of the expected type!
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public ref readonly TEvent UnsafeAsReadOnly<TEvent>()
			where TEvent : unmanaged, ICommonEvent<TEvent>
			=> ref TEvent.GetReference(ref Unsafe.AsRef(in @event));
	}
}
