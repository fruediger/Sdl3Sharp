using Sdl3Sharp.Utilities;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Provides extension methods for <see cref="Event"/>
/// </summary>
public static partial class EventExtensions
{
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
			where TEvent : struct, ICommonEvent<TEvent>
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
			where TEvent : struct, ICommonEvent<TEvent>
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
			where TEvent : struct, ICommonEvent<TEvent>
			=> TEvent.Accepts(@event.Type);

		/// <summary>
		/// Tries to reference an <see cref="Event"/> as a specific <em>read-only</em> <typeparamref name="TEvent"/>
		/// </summary>
		/// <typeparam name="TEvent">The type of event to cast the <see cref="Event"/> to</typeparam>
		/// <param name="result">The <see cref="Event"/> referenced as a specific <em>read-only</em> <typeparamref name="TEvent"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="NullableRefReadOnly{T}"/>)</c></param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="Event"/> is a <typeparamref name="TEvent"/> by it's <see cref="Event.Type"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool TryAsReadOnly<TEvent>(out NullableRefReadOnly<TEvent> result)
			where TEvent : struct, ICommonEvent<TEvent>
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
			where TEvent : struct, ICommonEvent<TEvent>
			=> ref TEvent.GetReference(ref Unsafe.AsRef(in @event));
	}
}
