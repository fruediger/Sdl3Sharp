using Sdl3Sharp.Timing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents the lifetime of a <see cref="UserEvent"/>, allowing to store managed data associated with it
/// </summary>
/// <remarks>
/// <para>
/// An <see cref="UserEventLifetime"/> allows to create <see cref="UserEvent"/>s that are able to participate in SDL's event system, while still being able to store managed data in their <see cref="UserEvent.Data1"/> and <see cref="UserEvent.Data2"/> properties.
/// </para>
/// <para>
/// <see cref="UserEventLifetime"/>s restrict the lifetime of managed data associated with <see cref="UserEvent"/>s created from them. 
/// Keep the <see cref="UserEventLifetime"/> instance alive as long as you need to access the managed data from the <see cref="UserEvent"/>s created from it.
/// Once you're sure that no more managed data access is needed, you can and should dispose the <see cref="UserEventLifetime"/> instance.
/// </para>
/// </remarks>
public sealed class UserEventLifetime : IDisposable
{
	private sealed class Shared { public object? Data { get; set; } = null; }

	/// <exception cref="ObjectDisposedException">The backing <see cref="UserEventLifetime"/> was already disposed</exception>
	[DoesNotReturn]
	private static object? FailDisposed() => throw new ObjectDisposedException(nameof(UserEventLifetime));

	/// <exception cref="InvalidOperationException">The <see cref="UserEvent"/> might not be backed by an <see cref="UserEventLifetime"/>, the backening <see cref="UserEventLifetime"/> was already disposed, or the corresponding RawData property of the <see cref="UserEvent"/> was overridden at some point</exception>
	[DoesNotReturn]	
	private static void FailUserEventNotBacked()
		=> throw new InvalidOperationException($"The {nameof(UserEvent)} might not be backed by an {nameof(UserEventLifetime)}, the backening {nameof(UserEventLifetime)} was already disposed, or the corresponding RawData property of the {nameof(UserEvent)} was overridden at some point.");

	private GCHandle mData1Handle, mData2Handle;

	/// <summary>
	/// Creates a new <see cref="UserEventLifetime"/>
	/// </summary>
	public UserEventLifetime()
	{
		mData1Handle = GCHandle.Alloc(new Shared(), GCHandleType.Normal);
		mData2Handle = GCHandle.Alloc(new Shared(), GCHandleType.Normal);
	}

	/// <inheritdoc/>
	~UserEventLifetime() => DisposeImpl();

	/// <summary>
	/// Gets or sets the value of the first <em>managed</em> user defined data slot, <see cref="UserEvent.Data1"/>, for <see cref="UserEvent"/>s created from this <see cref="UserEventLifetime"/>
	/// </summary>
	/// <value>
	/// The value of the first <em>managed</em> user defined data slot, <see cref="UserEvent.Data1"/>, for <see cref="UserEvent"/>s created from this <see cref="UserEventLifetime"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property sets the value of the <see cref="UserEvent.Data1"/> property for all <see cref="UserEvent"/>s created from this <see cref="UserEventLifetime"/>, as long as they still retained their original <see cref="UserEvent.RawData1"/> value.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="FailDisposed"/>
	public object? Data1
	{
		get
		{
			if (mData1Handle is { IsAllocated: true, Target: Shared { Data: var data } })
			{
				return data;
			}

			return FailDisposed();
		}

		set
		{
			if (mData1Handle is { IsAllocated: true, Target: Shared shared })
			{
				shared.Data = value;
			}

			FailDisposed();
		}
	}

	/// <summary>
	/// Gets or sets the value of the first <em>managed</em> user defined data slot, <see cref="UserEvent.Data2"/>, for <see cref="UserEvent"/>s created from this <see cref="UserEventLifetime"/>
	/// </summary>
	/// <value>
	/// The value of the first <em>managed</em> user defined data slot, <see cref="UserEvent.Data2"/>, for <see cref="UserEvent"/>s created from this <see cref="UserEventLifetime"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting the value of this property sets the value of the <see cref="UserEvent.Data2"/> property for all <see cref="UserEvent"/>s created from this <see cref="UserEventLifetime"/>, as long as they still retained their original <see cref="UserEvent.RawData2"/> value.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="FailDisposed"/>
	public object? Data2
	{
		get
		{
			if (mData2Handle is { IsAllocated: true, Target: Shared { Data: var data } })
			{
				return data;
			}

			return FailDisposed();
		}

		set
		{
			if (mData2Handle is { IsAllocated: true, Target: Shared shared })
			{
				shared.Data = value;
			}

			FailDisposed();
		}
	}

	/// <summary>
	/// Diposes the <see cref="UserEventLifetime"/>
	/// </summary>
	/// <remarks>
	/// <para>
	/// After disposing the <see cref="UserEventLifetime"/>, all <see cref="UserEvent"/>s created from it will no longer be able to access their <em>managed</em> data slots via the <see cref="UserEvent.Data1"/> and <see cref="UserEvent.Data2"/> properties.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		if (mData1Handle.IsAllocated)
		{
			mData1Handle.Free();
			mData1Handle = default;
		}

		if (mData2Handle.IsAllocated)
		{
			mData2Handle.Free();
			mData2Handle = default;
		}
	}

	/// <inheritdoc cref="FailUserEventNotBacked"/>
	internal static object? GetData1(ref readonly UserEvent @event)
	{
		if (!TryGetData1(in @event, out var data))
		{
			FailUserEventNotBacked();
		}

		return data;
	}

	/// <inheritdoc cref="FailUserEventNotBacked"/>
	internal static object? GetData2(ref readonly UserEvent @event)
	{
		if (!TryGetData2(in @event, out var data))
		{
			FailUserEventNotBacked();
		}

		return data;
	}

	/// <summary>
	/// Gets a new <see cref="UserEvent"/> backed by this <see cref="UserEventLifetime"/>
	/// </summary>
	/// <param name="type">The type of the event. This will become the value of the resulting <see cref="UserEvent"/>'s <see cref="UserEvent.Type"/> property.</param>
	/// <param name="timestamp">The timestamp of the event. You can populate this from <see cref="Timer.NanosecondTicks"/>. This will become the value of the resulting <see cref="UserEvent"/>'s <see cref="UserEvent.Timestamp"/> property.</param>
	/// <param name="windowId">The window Id of the <see cref="Window"/> associated with the event, or <c>0</c> if there is no associated window. This will become the value of the resulting <see cref="UserEvent"/>'s <see cref="UserEvent.WindowId"/> property.</param>
	/// <param name="code">The user defined event code. This will become the value of the resulting <see cref="UserEvent"/>'s <see cref="UserEvent.Code"/> property.</param>
	/// <returns>A new <see cref="UserEvent"/> backed by this <see cref="UserEventLifetime"/></returns>
	/// <remarks>
	/// <para>
	/// <paramref name="type"/> must be a valid value for <see cref="UserEvent"/>s.
	/// Otherwise, it will lead this method to throw an <see cref="ArgumentException"/>!
	/// Try registering user defined event type using <see cref="EventTypeExtensions.TryRegister(out EventType)"/> or <see cref="EventTypeExtensions.TryRegister(Span{EventType})"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException"><paramref name="type"/> was not valid for <see cref="UserEvent"/>s</exception>
	public UserEvent GetEvent(EventType type, ulong timestamp, uint windowId, int code)
		=> new()
		{
			Type = type,
			Timestamp = timestamp,
			WindowId = windowId,
			Code = code,
			RawData1 = GCHandle.ToIntPtr(mData1Handle),
			RawData2 = GCHandle.ToIntPtr(mData1Handle)
		};

	/// <inheritdoc cref="FailUserEventNotBacked"/>
	internal static void SetData1(ref readonly UserEvent @event, object? data)
	{
		if (!TrySetData1(in @event, data))
		{
			FailUserEventNotBacked();
		}
	}

	/// <inheritdoc cref="FailUserEventNotBacked"/>
	internal static void SetData2(ref readonly UserEvent @event, object? data)
	{
		if (!TrySetData1(in @event, data))
		{
			FailUserEventNotBacked();
		}
	}

	internal static bool TryGetData1(ref readonly UserEvent @event, out object? data)
	{
		if (@event.RawData1 is var rawData && rawData is not 0 && GCHandle.FromIntPtr(rawData) is { IsAllocated: true, Target: Shared shared })
		{
			data = shared.Data;
			return true;
		}

		data = default;
		return false;
	}

	internal static bool TryGetData2(ref readonly UserEvent @event, out object? data)
	{
		if (@event.RawData2 is var rawData && rawData is not 0 && GCHandle.FromIntPtr(rawData) is { IsAllocated: true, Target: Shared shared })
		{
			data = shared.Data;
			return true;
		}

		data = default;
		return false;
	}

	internal static bool TrySetData1(ref readonly UserEvent @event, object? data)
	{
		if (@event.RawData1 is var rawData && rawData is not 0 && GCHandle.FromIntPtr(rawData) is { IsAllocated: true, Target: Shared shared })
		{
			shared.Data = data;
			return true;
		}

		return false;
	}

	internal static bool TrySetData2(ref readonly UserEvent @event, object? data)
	{
		if (@event.RawData2 is var rawData && rawData is not 0 && GCHandle.FromIntPtr(rawData) is { IsAllocated: true, Target: Shared shared })
		{
			shared.Data = data;
			return true;
		}

		return false;
	}
}
