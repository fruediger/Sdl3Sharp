using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.Internal.Interop.NativeImportConditions;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Threading;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using unsafe SDL_iOSAnimationCallback = delegate* unmanaged[Cdecl]<void*, void>;
using unsafe SDL_RequestAndroidPermissionCallback = delegate* unmanaged[Cdecl]<void*, byte*, Sdl3Sharp.Internal.Interop.CBool, void>;
using unsafe SDL_WindowsMessageHook = delegate* unmanaged[Cdecl]<void*, void*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_X11EventHook = delegate* unmanaged[Cdecl]<void*, void*, Sdl3Sharp.Internal.Interop.CBool>;

namespace Sdl3Sharp.Utilities;

partial class Platform
{
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static void IOSAnimationCallback(void* userdata)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: IOSAnimationCallback callback })
		{
			callback();
		}
	}

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static void RequestAndroidPermissionCallback(void* userdata, byte* permission, CBool granted)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: AndroidRequestPermissionCallback callback } gcHandle)
		{
			try
			{
				callback(Utf8StringMarshaller.ConvertToManaged(permission)!, granted);
			}
			finally
			{
				gcHandle.Free();
			}
		}
	}

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static CBool WindowsMessageHook(void* userdata, void* msg)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: WindowsMessageHook hook })
		{
			return hook(unchecked((IntPtr)msg));
		}

		return true; // default to let the message continue on
	}

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static CBool X11EventHook(void* userdata, void* xevent)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: X11EventHook hook })
		{
			return hook(unchecked((IntPtr)xevent));
		}

		return true; // default to let the event continue on
	}

	/// <summary>
	/// Retrieve the Java instance of the Android activity class
	/// </summary>
	/// <returns>Returns the jobject representing the instance of the Activity class of the Android application, or NULL on failure;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The prototype of the function in SDL's code actually declares a void* return type, even if the implementation returns a jobject.
	/// The rationale being that the SDL headers can avoid including jni.h.
	/// </para>
	/// <para>
	/// The jobject returned by the function is a local reference and must be released by the caller.
	/// See the PushLocalFrame() and PopLocalFrame() or DeleteLocalRef() functions of the Java native interface:
	/// <see href="https://docs.oracle.com/javase/1.5.0/docs/guide/jni/spec/functions.html"/>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidActivity">SDL_GetAndroidActivity</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_GetAndroidActivity();

	/// <summary>
	/// Get the path used for caching data for this Android application
	/// </summary>
	/// <returns>Returns the path used for caches for this application on success or NULL on failure;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This path is unique to your application, but is public and can be written to by other applications.
	/// </para>
	/// <para>
	/// Your cache path is typically: <c>/data/data/your.app.package/cache/</c>.
	/// </para>
	/// <para>
	/// This is a C wrapper over <c>android.content.Context.getCacheDir()</c>: <see href="https://developer.android.com/reference/android/content/Context#getCacheDir()"/>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidCachePath">SDL_GetAndroidCachePath</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetAndroidCachePath();

	/// <summary>
	/// Get the path used for external storage for this Android application
	/// </summary>
	/// <returns>Returns the path used for external storage for this application on success or NULL on failure;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This path is unique to your application, but is public and can be written to by other applications.
	/// </para>
	/// <para>
	/// Your external storage path is typically: <c>/storage/sdcard0/Android/data/your.app.package/files</c>.
	/// </para>
	/// <para>
	/// This is a C wrapper over <c>android.content.Context.getExternalFilesDir()</c>: <see href="https://developer.android.com/reference/android/content/Context#getExternalFilesDir()"/>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidExternalStoragePath">SDL_GetAndroidExternalStoragePath</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetAndroidExternalStoragePath();

	/// <summary>
	/// Get the current state of external storage for this Android application
	/// </summary>
	/// <returns>Returns the current state of external storage, or 0 if external storage is currently unavailable</returns>
	/// <remarks>
	/// <para>
	/// The current state of external storage, a bitmask of these values:
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_ANDROID_EXTERNAL_STORAGE_READ"><c>SDL_ANDROID_EXTERNAL_STORAGE_READ</c></see>,
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_ANDROID_EXTERNAL_STORAGE_WRITE"><c>SDL_ANDROID_EXTERNAL_STORAGE_WRITE</c></see>.
	/// </para>
	/// <para>
	/// If external storage is currently unavailable, this will return 0.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidExternalStorageState">SDL_GetAndroidExternalStorageState</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial AndroidExternalStorageState SDL_GetAndroidExternalStorageState();

	/// <summary>
	/// Get the path used for internal storage for this Android application
	/// </summary>
	/// <returns>Returns the path used for internal storage or NULL on failure;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This path is unique to your application and cannot be written to by other applications.
	/// </para>
	/// <para>
	/// Your internal storage path is typically: <c>/data/data/your.app.package/files</c>.
	/// </para>
	/// <para>
	/// This is a C wrapper over <c>android.content.Context.getFilesDir()</c>: <see href="https://developer.android.com/reference/android/content/Context#getFilesDir()"/>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidInternalStoragePath">SDL_GetAndroidInternalStoragePath</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetAndroidInternalStoragePath();

	/// <summary>
	/// Get the Android Java Native Interface Environment of the current thread
	/// </summary>
	/// <returns>Returns a pointer to Java native interface object (JNIEnv) to which the current thread is attached, or NULL on failure;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is the JNIEnv one needs to access the Java virtual machine from native code, and is needed for many Android APIs to be usable from C.
	/// </para>
	/// <para>
	/// The prototype of the function in SDL's code actually declare a void* return type, even if the implementation returns a pointer to a JNIEnv.
	/// The rationale being that the SDL headers can avoid including jni.h.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidJNIEnv">SDL_GetAndroidJNIEnv</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_GetAndroidJNIEnv();

	/// <summary>
	/// Query Android API level of the current device
	/// </summary>
	/// <returns>Returns the Android API level</returns>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	/// <item>
	///		<term>API level 35</term>
	///		<description>Android 15 (VANILLA_ICE_CREAM)</description>
	/// </item>
	/// <item>
	///		<term>API level 34</term>
	///		<description>Android 14 (UPSIDE_DOWN_CAKE)</description>
	/// </item>
	/// <item>
	///		<term>API level 33</term>
	///		<description>Android 13 (TIRAMISU)</description>
	/// </item>
	/// <item>
	///		<term>API level 32</term>
	///		<description>Android 12L (S_V2)</description>
	/// </item>
	/// <item>
	///		<term>API level 31</term>
	///		<description>Android 12 (S)</description>
	/// </item>
	/// <item>
	///		<term>API level 30</term>
	///		<description>Android 11 (R)</description>
	/// </item>
	/// <item>
	///		<term>API level 29</term>
	///		<description>Android 10 (Q)</description>
	/// </item>
	/// <item>
	///		<term>API level 28</term>
	///		<description>Android 9 (P)</description>
	/// </item>
	/// <item>
	///		<term>API level 27</term>
	///		<description>Android 8.1 (O_MR1)</description>
	/// </item>
	/// <item>
	///		<term>API level 26</term>
	///		<description>Android 8.0 (O)</description>
	/// </item>
	/// <item>
	///		<term>API level 25</term>
	///		<description>Android 7.1 (N_MR1)</description>
	/// </item>
	/// <item>
	///		<term>API level 24</term>
	///		<description>Android 7.0 (N)</description>
	/// </item>
	/// <item>
	///		<term>API level 23</term>
	///		<description>Android 6.0 (M)</description>
	/// </item>
	/// <item>
	///		<term>API level 22</term>
	///		<description>Android 5.1 (LOLLIPOP_MR1)</description>
	/// </item>
	/// <item>
	///		<term>API level 21</term>
	///		<description>Android 5.0 (LOLLIPOP, L)</description>
	/// </item>
	/// <item>
	///		<term>API level 20</term>
	///		<description>Android 4.4W (KITKAT_WATCH)</description>
	/// </item>
	/// <item>
	///		<term>API level 19</term>
	///		<description>Android 4.4 (KITKAT)</description>
	/// </item>
	/// <item>
	///		<term>API level 18</term>
	///		<description>Android 4.3 (JELLY_BEAN_MR2)</description>
	/// </item>
	/// <item>
	///		<term>API level 17</term>
	///		<description>Android 4.2 (JELLY_BEAN_MR1)</description>
	/// </item>
	/// <item>
	///		<term>API level 16</term>
	///		<description>Android 4.1 (JELLY_BEAN)</description>
	/// </item>
	/// <item>
	///		<term>API level 15</term>
	///		<description>Android 4.0.3 (ICE_CREAM_SANDWICH_MR1)</description>
	/// </item>
	/// <item>
	///		<term>API level 14</term>
	///		<description>Android 4.0 (ICE_CREAM_SANDWICH)</description>
	/// </item>
	/// <item>
	///		<term>API level 13</term>
	///		<description>Android 3.2 (HONEYCOMB_MR2)</description>
	/// </item>
	/// <item>
	///		<term>API level 12</term>
	///		<description>Android 3.1 (HONEYCOMB_MR1)</description>
	/// </item>
	/// <item>
	///		<term>API level 11</term>
	///		<description>Android 3.0 (HONEYCOMB)</description>
	/// </item>
	/// <item>
	///		<term>API level 10</term>
	///		<description>Android 2.3.3 (GINGERBREAD_MR1)</description>
	/// </item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAndroidSDKVersion">SDL_GetAndroidSDKVersion</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetAndroidSDKVersion();

	/// <summary>
	/// Get the D3D9 adapter index that matches the specified display
	/// </summary>
	/// <param name="displayID">The instance of the display to query</param>
	/// <returns>Returns the D3D9 adapter index on success or -1 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The returned adapter index can be passed to <c>IDirect3D9::CreateDevice</c> and controls on which monitor a full screen application will appear.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDirect3D9AdapterIndex">SDL_GetDirect3D9AdapterIndex</seealso>
	[SupportedOSPlatform("Windows")]
	[NativeImportFunction<Library, OrElse<IsWin32, IsWinGDK>>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetDirect3D9AdapterIndex(uint displayID);

	/// <summary>
	/// Get the DXGI Adapter and Output indices for the specified display
	/// </summary>
	/// <param name="displayID">The instance of the display to query</param>
	/// <param name="adapterIndex">A pointer to be filled in with the adapter index</param>
	/// <param name="outputIndex">A pointer to be filled in with the output index</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The DXGI Adapter and Output indices can be passed to <c>EnumAdapters</c> and <c>EnumOutputs</c> respectively to get the objects required to create a DX10 or DX11 device and swap chain.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDXGIOutputInfo">SDL_GetDXGIOutputInfo</seealso>
	[SupportedOSPlatform("Windows")]
	[NativeImportFunction<Library, OrElse<IsWin32, IsWinGDK>>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetDXGIOutputInfo(uint displayID, int* adapterIndex, int* outputIndex);

	/// <summary>
	/// Gets a reference to the default user handle for GDK
	/// </summary>
	/// <param name="outUserHandle">A pointer to be filled in with the default user handle</param>
	/// <returns>Returns true if success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This is effectively a synchronous version of XUserAddAsync, which always prefers the default user and allows a sign-in UI.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetGDKDefaultUser">SDL_GetGDKDefaultUser</seealso>
	[NativeImportFunction<Library, IsGDK>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetGDKDefaultUser(void** outUserHandle);

	/// <summary>
	/// Gets a reference to the global async task queue handle for GDK, initializing if needed
	/// </summary>
	/// <param name="outTaskQueue">A pointer to be filled in with task queue handle</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// Once you are done with the task queue, you should call XTaskQueueCloseHandle to reduce the reference count to avoid a resource leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetGDKTaskQueue">SDL_GetGDKTaskQueue</seealso>
	[NativeImportFunction<Library, IsGDK>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetGDKTaskQueue(void** outTaskQueue);

	/// <summary>
	/// Get the name of the platform
	/// </summary>
	/// <returns>Returns the name of the platform. If the correct platform name is not available, returns a string beginning with the text "Unknown"</returns>
	/// <remarks>
	/// <para>
	/// Here are the names returned for some (but not all) supported platforms:
	/// <list type="bullet">
	/// <item><description>"Windows"</description></item>
	/// <item><description>"macOS"</description></item>
	/// <item><description>"Linux"</description></item>
	/// <item><description>"iOS"</description></item>
	/// <item><description>"Android"</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPlatform">SDL_GetPlatform</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetPlatform();

	/// <summary>
	/// Get the application sandbox environment, if any
	/// </summary>
	/// <returns>Returns the application sandbox environment or <see href="https://wiki.libsdl.org/SDL3/SDL_SANDBOX_NONE">SDL_SANDBOX_NONE</see> if the application is not running in a sandbox environment</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSandbox">SDL_GetSandbox</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial Sandbox SDL_GetSandbox();

	/// <summary>
	/// Query if the application is running on a Chromebook
	/// </summary>
	/// <returns>Returns true if this is a Chromebook, false otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IsChromebook"></seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_IsChromebook();

	/// <summary>
	/// Query if the application is running on a Samsung DeX docking station
	/// </summary>
	/// <returns>Returns true if this is a DeX docking station, false otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IsDeXMode">SDL_IsDeXMode</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_IsDeXMode();

	/// <summary>
	/// Query if the current device is a tablet
	/// </summary>
	/// <returns>Returns true if the device is a tablet, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If SDL can't determine this, it will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IsTablet">SDL_IsTablet</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_IsTablet();

	/// <summary>
	/// Query if the current device is a TV
	/// </summary>
	/// <returns>Returns true if the device is a TV, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If SDL can't determine this, it will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IsTV">SDL_IsTV</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_IsTV();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationDidChangeStatusBarOrientation
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidChangeStatusBarOrientation"></seealso>
	[SupportedOSPlatform("iOS")]
	[NativeImportFunction<Library, IsIOS>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationDidChangeStatusBarOrientation();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationDidEnterBackground
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidEnterBackground"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationDidEnterBackground();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationDidBecomeActive
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidEnterBackground"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationDidEnterForeground();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationDidReceiveMemoryWarning
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidEnterBackground"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationDidReceiveMemoryWarning();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationWillResignActive
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidEnterBackground"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationWillEnterBackground();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationWillEnterForeground
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidEnterBackground"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationWillEnterForeground();

	/// <summary>
	/// Let iOS apps with external event handling report onApplicationWillTerminate
	/// </summary>
	/// <remarks>
	/// <para>
	/// This functions allows iOS apps that have their own event handling to hook into SDL to generate SDL events.
	/// This maps directly to an iOS-specific event, but since it doesn't do anything iOS-specific internally, it is available on all platforms, in case it might be useful for some specific paradigm.
	/// Most apps do not need to use this directly; SDL's internal event code will handle all this for windows created by <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OnApplicationDidEnterBackground"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_OnApplicationWillTerminate();

	/// <summary>
	/// Request permissions at runtime, asynchronously
	/// </summary>
	/// <param name="permission">The permission to request</param>
	/// <param name="cb">The callback to trigger when the request has a response</param>
	/// <param name="userdata">An app-controlled pointer that is passed to the callback</param>
	/// <returns>
	/// Returns true if the request was submitted, false if there was an error submitting.
	/// The result of the request is only ever reported through the callback, not this return value.
	/// </returns>
	/// <remarks>
	/// <para>
	/// You do not need to call this for built-in functionality of SDL;
	/// recording from a microphone or reading images from a camera, using standard SDL APIs, will manage permission requests for you.
	/// </para>
	/// <para>
	/// This function never blocks. Instead, the app-supplied callback will be called when a decision has been made.
	/// This callback may happen on a different thread, and possibly much later, as it might wait on a user to respond to a system dialog.
	/// If permission has already been granted for a specific entitlement, the callback will still fire, probably on the current thread and before this function returns.
	/// </para>
	/// <para>
	/// If the request submission fails, this function returns -1 and the callback will NOT be called, but this should only happen in catastrophic conditions, like memory running out.
	/// Normally there will be a yes or no to the request through the callback.
	/// </para>
	/// <para>
	/// For the <c><paramref name="permission"/></c> parameter, choose a value from here: <see href="https://developer.android.com/reference/android/Manifest.permission"/>	/// 
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RequestAndroidPermission">SDL_RequestAndroidPermission</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RequestAndroidPermission(byte* permission, SDL_RequestAndroidPermissionCallback cb, void* userdata);

	/// <summary>
	/// Trigger the Android system back button behavior
	/// </summary>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SendAndroidBackButton">SDL_SendAndroidBackButton</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_SendAndroidBackButton();

	/// <summary>
	/// Send a user command to SDLActivity
	/// </summary>
	/// <param name="command">User command that must be greater or equal to 0x8000</param>
	/// <param name="param">User parameter</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// Override "boolean onUnhandledMessage(Message msg)" to handle the message.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SendAndroidMessage">SDL_SendAndroidMessage</seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_SendAndroidMessage(uint command, int param);

	/// <summary>
	/// Use this function to set the animation callback on Apple iOS
	/// </summary>
	/// <param name="window">The window for which the animation callback should be set</param>
	/// <param name="interval">The number of frames after which <c><paramref name="callback"/></c> will be called</param>
	/// <param name="callback">The function to call for every frame</param>
	/// <param name="callbackParam">A pointer that is passed to <c><paramref name="callback"/></c></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The function prototype for <c><paramref name="callback"/></c> is:
	/// <code>
	///	void callback(void *callbackParam);
	/// </code>
	/// Where its parameter, <c>callbackParam</c>, is what was passed as <c><paramref name="callbackParam"/></c> to <see href="https://wiki.libsdl.org/SDL3/SDL_SetiOSAnimationCallback">SDL_SetiOSAnimationCallback</see>().
	/// </para>
	/// <para>
	/// This function is only available on Apple iOS.
	/// </para>
	/// <para>
	/// For more information see: <see href="https://wiki.libsdl.org/SDL3/README-ios"/>.
	/// </para>
	/// <para>
	/// Note that if you use the "main callbacks" instead of a standard C <c>main</c> function, you don't have to use this API, as SDL will manage this for you.
	/// </para>
	/// <para>
	/// Details on main callbacks are here: <see href="https://wiki.libsdl.org/SDL3/README-main-functions"/>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetiOSAnimationCallback">SDL_SetiOSAnimationCallback</seealso>
	[SupportedOSPlatform("iOS")]
	[NativeImportFunction<Library, IsIOS>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetiOSAnimationCallback(Window.SDL_Window* window, int interval, SDL_iOSAnimationCallback callback, void* callbackParam);

	/// <summary>
	/// Use this function to enable or disable the SDL event pump on Apple iOS
	/// </summary>
	/// <param name="enabled">true to enable the event pump, false to disable it</param>
	/// <remarks>
	/// <para>
	/// This function is only available on Apple iOS.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetiOSEventPump">SDL_SetiOSEventPump</seealso>
	[SupportedOSPlatform("iOS")]
	[NativeImportFunction<Library, IsIOS>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_SetiOSEventPump(CBool enabled);

	/// <summary>
	/// Sets the UNIX nice value for a thread
	/// </summary>
	/// <param name="threadID">The Unix thread ID to change priority of</param>
	/// <param name="priority">The new, Unix-specific, priority value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This uses setpriority() if possible, and RealtimeKit if available.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetLinuxThreadPriority">SDL_SetLinuxThreadPriority</seealso>
	[SupportedOSPlatform("Linux")]
	[NativeImportFunction<Library, IsLinux>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_SetLinuxThreadPriority(long threadID, int priority);

	/// <summary>
	/// Sets the priority (not nice level) and scheduling policy for a thread
	/// </summary>
	/// <param name="threadID">The Unix thread ID to change priority of</param>
	/// <param name="sdlPriority">The new <see href="https://wiki.libsdl.org/SDL3/SDL_ThreadPriority">SDL_ThreadPriority</see> value</param>
	/// <param name="schedPolicy">The new scheduling policy (SCHED_FIFO, SCHED_RR, SCHED_OTHER, etc...)</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This uses setpriority() if possible, and RealtimeKit if available.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetLinuxThreadPriorityAndPolicy">SDL_SetLinuxThreadPriorityAndPolicy</seealso>
	[SupportedOSPlatform("Linux")]
	[NativeImportFunction<Library, IsLinux>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_SetLinuxThreadPriorityAndPolicy(long threadID, ThreadPriority sdlPriority, int schedPolicy);

	/// <summary>
	/// Set a callback for every Windows message, run before TranslateMessage()
	/// </summary>
	/// <param name="callback">The <see href="https://wiki.libsdl.org/SDL3/SDL_WindowsMessageHook">SDL_WindowsMessageHook</see> function to call</param>
	/// <param name="userdata">A pointer to pass to every iteration of <c><paramref name="callback"/></c></param>
	/// <remarks>
	/// <para>
	/// The callback may modify the message, and should return true if the message should continue to be processed, or false to prevent further processing.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetWindowsMessageHook">SDL_SetWindowsMessageHook</seealso>
	[SupportedOSPlatform("Windows")]
	[NativeImportFunction<Library, IsWindows>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_SetWindowsMessageHook(SDL_WindowsMessageHook callback, void* userdata);

	/// <summary>
	/// Set a callback for every X11 event
	/// </summary>
	/// <param name="callback">The <see href="https://wiki.libsdl.org/SDL3/SDL_X11EventHook">SDL_X11EventHook</see> function to call.</param>
	/// <param name="userdata">A pointer to pass to every iteration of <c><paramref name="callback"/></c></param>
	/// <remarks>
	/// <para>
	/// The callback may modify the event, and should return true if the event should continue to be processed, or false to prevent further processing.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetX11EventHook">SDL_SetX11EventHook</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_SetX11EventHook(SDL_X11EventHook callback, void* userdata);

	/// <summary>
	/// Shows an Android toast notification
	/// </summary>
	/// <param name="message">Text message to be shown</param>
	/// <param name="duration">0=short, 1=long</param>
	/// <param name="gravity">Where the notification should appear on the screen</param>
	/// <param name="xoffset">Set this parameter only when gravity &gt;=0</param>
	/// <param name="yoffset">Set this parameter only when gravity &gt;=0</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Toasts are a sort of lightweight notification that are unique to Android. <see href="https://developer.android.com/guide/topics/ui/notifiers/toasts"/>.
	/// </para>
	/// <para>
	/// Shows toast in UI thread.
	/// </para>
	/// <para>
	/// For the <c><paramref name="gravity"/></c> parameter, choose a value from here, or -1 if you don't have a preference: <see href="https://developer.android.com/reference/android/view/Gravity"/>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowAndroidToast"></seealso>
	[SupportedOSPlatform("Android")]
	[NativeImportFunction<Library, IsAndroid>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ShowAndroidToast(byte* message, int duration, int gravity, int xoffset, int yoffset);
}
