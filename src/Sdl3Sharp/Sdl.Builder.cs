using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

partial class Sdl
{
	/// <summary>
	/// A builder that lets you perfom some preliminaries right before SDL gets initialized 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public readonly ref struct Builder
	{
		private readonly Sdl mSdl;
		private readonly ref SubSystemSet mSubSystems;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		internal Builder(Sdl sdl, ref SubSystemSet subSystems) { mSdl = sdl; mSubSystems = ref subSystems; }

		/// <summary>
		/// Sets certain <see cref="SubSystem">sub systems</see> to be <em>not</em> initialized with SDL
		/// </summary>
		/// <param name="subSystems">A set of <see cref="SubSystem">sub systems</see> to be <em>not</em> initialized with SDL</param>
		/// <returns>The current <see cref="Builder"/> so that additional calls can be chained</returns>
		/// <remarks>
		/// NOTE: This does not prevent depend sub system from being initialized (e.g. <see cref="SubSystem.Events"/> when <see cref="SubSystem.Audio"/> should be initialized)
		/// </remarks>
		/// <inheritdoc cref="Validate"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Builder DontInitializeSubSystems(params SubSystemSet subSystems)
		{
			Validate();

			mSubSystems &= ~subSystems;

			return this;
		}

		/// <summary>
		/// Sets certain <see cref="SubSystem">sub systems</see> to be initialized with SDL
		/// </summary>
		/// <param name="subSystems">A set of <see cref="SubSystem">sub systems</see> to be initialized with SDL</param>
		/// <returns>The current <see cref="Builder"/> so that additional calls can be chained</returns>
		/// <inheritdoc cref="Validate"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Builder InitializeSubSystems(params SubSystemSet subSystems)
		{
			Validate();

			mSubSystems |= subSystems;

			return this;
		}


		/// <summary>
		/// Tries to set metadata about your app
		/// </summary>
		/// <param name="name">The name of the metadata</param>
		/// <param name="value">The value of the metadata, or <c><see langword="null"/></c> to remove that metadata</param>
		/// <returns><c><see langword="true"/></c> if the value of the metadata was successfully set; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
		/// </para>
		/// <para>
		/// There are several locations where SDL can make use of metadata (an "About" box in the macOS menu bar, the name of the app can be shown on some audio mixers, etc).
		/// Any piece of metadata can be left out, if a specific detail doesn't make sense for the app.
		/// </para>
		/// <para>
		/// See <see cref="Metadata"/> for a overview over the available metadata properties and their meanings.
		/// </para>
		/// <para>
		/// Multiple calls to this method with the same <paramref name="name"/> value are allowed, but various state might not change once it has been already set up.
		/// </para>
		/// </remarks>
		/// <inheritdoc cref="Validate"/>
		public readonly bool TrySetMetadata(string name, string? value)
		{
			Validate();

			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);
				var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);

				try
				{
					return SDL_SetAppMetadataProperty(nameUtf8, valueUtf8);
				}
				finally
				{
					Utf8StringMarshaller.Free(valueUtf8);
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		/// <summary>
		/// Tries to set basic metadata about your app
		/// </summary>
		/// <param name="appName">The name of the application (<c>"My Game 2: Bad Guy's Revenge!"</c>)</param>
		/// <param name="appVersion">The version of the application (<c>"1.0.0beta5"</c> or a git hash, or whatever makes sense)</param>
		/// <param name="appIdentifier">A unique string in reverse-domain format that identifies this app (<c>"com.example.mygame2"</c>)</param>
		/// <returns><c><see langword="true"/></c> if the value of the metadata values are successfully set; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
		/// </para>
		/// <para>
		/// There are several locations where SDL can make use of metadata (an "About" box in the macOS menu bar, the name of the app can be shown on some audio mixers, etc).
		/// Any piece of metadata can be left out as a <c><see langword="null"/></c> value, if a specific detail doesn't make sense for the app.
		/// </para>
		/// <para>
		/// Passing a <c><see langword="null"/></c> value removes any previous metadata.
		/// </para>
		/// <para>
		/// Multiple calls to this method are allowed, but various state might not change once it has been already set up.
		/// </para>
		/// <para>
		/// This is a simplified interface for the most important information. You can supply significantly more detailed metadata with <see cref="TrySetMetadata(string, string?)"/>.
		/// </para>
		/// </remarks>
		/// <inheritdoc cref="Validate"/>
		public readonly bool TrySetMetadata(string? appName, string? appVersion, string? appIdentifier)
		{
			Validate();

			unsafe
			{
				var appNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(appName);
				var appVersionUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(appVersion);
				var appIdentifierUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(appIdentifier);

				try
				{
					return SDL_SetAppMetadata(appNameUtf8, appVersionUtf8, appIdentifierUtf8);
				}
				finally
				{
					Utf8StringMarshaller.Free(appIdentifierUtf8);
					Utf8StringMarshaller.Free(appVersionUtf8);
					Utf8StringMarshaller.Free(appNameUtf8);
				}
			}
		}

		/// <exception cref="InvalidOperationException">The <see cref="Builder"/> is used outside of the initialization process of a <see cref="Sdl"/> instance</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		[MemberNotNull(nameof(mSdl))]
		private void Validate()
		{
			if (mSdl?.mLifetimeState is not LifetimeState.Initializing || Unsafe.IsNullRef(ref mSubSystems))
			{
				failInvalidBuilder();
			}

			[DoesNotReturn]
			static void failInvalidBuilder() => throw new InvalidOperationException($"The {nameof(Builder)} is used outside of the initialization process of a {nameof(Sdl)} instance");
		}
	}
}
