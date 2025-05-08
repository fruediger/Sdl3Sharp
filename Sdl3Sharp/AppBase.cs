using Sdl3Sharp.Events;

namespace Sdl3Sharp;

/// <summary>
/// A base class for an app's lifetime model
/// </summary>
public abstract partial class AppBase
{
	/// <summary>
	/// Gets <see cref="AppResult.Continue"/>.
	/// This is thought of as a convenient shorthand property for <see cref="AppResult.Continue"/> used by types extending from <see cref="AppBase"/>. 
	/// </summary>
	/// <value><see cref="AppResult.Continue"/></value>
	protected static AppResult Continue => AppResult.Continue;

	/// <summary>
	/// Gets <see cref="AppResult.Success"/>.
	/// This is thought of as a convenient shorthand property for <see cref="AppResult.Success"/> used by types extending from <see cref="AppBase"/>. 
	/// </summary>
	/// <value><see cref="AppResult.Success"/></value>
	protected static AppResult Success => AppResult.Success;

	/// <summary>
	/// Gets <see cref="AppResult.Failure"/>.
	/// This is thought of as a convenient shorthand property for <see cref="AppResult.Failure"/> used by types extending from <see cref="AppBase"/>. 
	/// </summary>
	/// <value><see cref="AppResult.Failure"/></value>
	protected static AppResult Failure => AppResult.Failure;

	/// <summary>
	/// The initial entry point for an <see cref="AppBase"/>'s execution
	/// </summary>
	/// <param name="sdl">The <see cref="Sdl"/> instance that started the execution of this <see cref="AppBase"/></param>
	/// <param name="args">A <em>further processed</em> collection of arguments passed from <see cref="Sdl.Run(AppBase, System.ReadOnlySpan{string})"/> or <see cref="Sdl.Run{TArguments}(AppBase, TArguments)"/></param>
	/// <returns>
	/// How to proceed with the operation of this <see cref="AppBase"/>'s execution:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="AppResult.Failure"/></term>
	///			<description>Terminate the execution with an error</description>
	///		</item>
	///		<item>
	///			<term><see cref="AppResult.Success"/></term>
	///			<description>Terminate the execution with success</description>
	///		</item>
	///		<item>
	///			<term><see cref="AppResult.Continue"/></term>
	///			<description>Continue with the execution</description>
	///		</item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This method is called once, at the start of the <see cref="AppBase"/>'s execution.
	/// It should initialize whatever is necessary, possibly create windows and open audio devices, etc.
	/// </para>
	/// <para>
	/// This method should not go into an infinite loop; it should do any one-time setup it requires and then return.
	/// </para>
	/// <para>
	/// The <paramref name="args"/> parameter contains almost the same collection of arguments as the caller passed to <see cref="Sdl.Run(AppBase, System.ReadOnlySpan{string})"/> or <see cref="Sdl.Run{TArguments}(AppBase, TArguments)"/>.
	/// <em>But notice that SDL does further (sometimes platform depended) processing on those arguments between the calls to those methods and the call to this method.</em>
	/// You should treat those arguments as you normally would main entry point arguments.
	/// </para>
	/// <para>
	/// If this method returns <see cref="AppResult.Continue"/>, the <see cref="AppBase"/>'s execution will proceed to normal operation, and will begin receiving repeated calls to <see cref="OnIterateInternal(Sdl)"/> and <see cref="OnEvent(Sdl, ref readonly Event)"/> for the execution time of the <see cref="AppBase"/>.
	/// If this method returns <see cref="AppResult.Failure"/>, <see cref="OnQuit(Sdl, AppResult)"/> will be called immediately and terminate the <see cref="AppBase"/>'s execution with a return value that can be used as an exit code for program that reports an error to the platform.
	/// If it returns <see cref="AppResult.Success"/>, <see cref="OnQuit(Sdl, AppResult)"/> will be called immediately and terminate the <see cref="AppBase"/>'s execution with a return value that can be used as an exit code for the program that reports success to the platform.
	/// </para>
	/// <para>
	/// This method is called by SDL on the main thread.
	/// </para>
	/// </remarks>
	protected virtual AppResult OnInitialize(Sdl sdl, string[] args) => Continue;

	/// <summary>
	/// The iteration entry point for an <see cref="AppBase"/>'s execution
	/// </summary>
	/// <param name="sdl">The <see cref="Sdl"/> instance that started the execution of this <see cref="AppBase"/></param>
	/// <returns>
	/// How to proceed with the operation of this <see cref="AppBase"/>'s execution:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="AppResult.Failure"/></term>
	///			<description>Terminate the execution with an error</description>
	///		</item>
	///		<item>
	///			<term><see cref="AppResult.Success"/></term>
	///			<description>Terminate the execution with success</description>
	///		</item>
	///		<item>
	///			<term><see cref="AppResult.Continue"/></term>
	///			<description>Continue with the execution</description>
	///		</item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This method is called repeatedly after <see cref="OnInitialize(Sdl, string[])"/> returned <see cref="AppResult.Continue"/>.
	/// The method should operate as a single iteration the <see cref="AppBase"/>'s primary loop; it should update whatever state it needs and draw a new frame of video, usually.
	/// </para>
	/// <para>
	/// On some platforms, this method will be called at the refresh rate of the display (which might change during the lifetime of your app!).
	/// There are no promises made about what frequency this method might run at.
	/// You should use the <see cref="Timing.Timer"/> functionality (e.g. <see cref="Timing.Timer.MillisecondTicks"/>) if you need to see how much time has passed since the last iteration.
	/// </para>
	/// <para>
	/// There is no need to process events during this method; events will be send to <see cref="OnEvent(Sdl, ref readonly Event)"/> as they arrive, and in most cases the event queue will be empty when this method runs anyhow.
	/// </para>
	/// <para>
	/// This method should not go into an infinite loop; it should do one iteration of whatever it needs to do and return.
	/// </para>
	/// <para>
	/// If this method returns <see cref="AppResult.Continue"/>, the <see cref="AppBase"/>'s execution will continue normal operation, receiving repeated calls to <see cref="OnIterateInternal(Sdl)"/> and <see cref="OnEvent(Sdl, ref readonly Event)"/> for the execution time of the <see cref="AppBase"/>.
	/// If this method returns <see cref="AppResult.Failure"/>, <see cref="OnQuit(Sdl, AppResult)"/> will be called immediately and terminate the <see cref="AppBase"/>'s execution with a return value that can be used as an exit code for program that reports an error to the platform.
	/// If it returns <see cref="AppResult.Success"/>, <see cref="OnQuit(Sdl, AppResult)"/> will be called immediately and terminate the <see cref="AppBase"/>'s execution with a return value that can be used as an exit code for the program that reports success to the platform.
	/// </para>
	/// <para>
	/// This method is called by SDL on the main thread.
	/// </para>
	/// </remarks>
	protected virtual AppResult OnIterate(Sdl sdl) => Success;

	/// <summary>
	/// The event handling entry point for an <see cref="AppBase"/>'s execution
	/// </summary>
	/// <param name="sdl">The <see cref="Sdl"/> instance that started the execution of this <see cref="AppBase"/></param>
	/// <param name="event">The newly arrived event to examine</param>
	/// <returns>
	/// How to proceed with the operation of this <see cref="AppBase"/>'s execution:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="AppResult.Failure"/></term>
	///			<description>Terminate the execution with an error</description>
	///		</item>
	///		<item>
	///			<term><see cref="AppResult.Success"/></term>
	///			<description>Terminate the execution with success</description>
	///		</item>
	///		<item>
	///			<term><see cref="AppResult.Continue"/></term>
	///			<description>Continue with the execution</description>
	///		</item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This method is called as needed after <see cref="OnInitialize(Sdl, string[])"/> returned <see cref="AppResult.Continue"/>.
	/// It is called once for each new event.
	/// </para>
	/// <para>
	/// There is (currently) no guarantee about what thread this will be called from; whatever thread pushes an event onto SDL's event queue will trigger a call to this method.
	/// SDL is responsible for pumping the event queue between each call to <see cref="OnIterate(Sdl)"/>, so in normal operation one should only get events in a serial fashion,
	/// but be careful if you have a thread that explicitly calls <see cref="SDL_PushEvent"/>.
	/// SDL itself will push events to the queue on the main thread.
	/// </para>
	/// <para>
	/// <see cref="Event"/>s sent to this method by reference are not owned (by you), nor are those references required to be alive after the call to this method;
	/// if you need to save the data, you should copy it!
	/// </para>
	/// <para>
	/// This method should not go into an infinite loop; it should handle the provided event appropriately and return.
	/// </para>
	/// <para>
	/// If this method returns <see cref="AppResult.Continue"/>, the <see cref="AppBase"/>'s execution will continue normal operation, receiving repeated calls to <see cref="OnIterateInternal(Sdl)"/> and <see cref="OnEvent(Sdl, ref readonly Event)"/> for the execution time of the <see cref="AppBase"/>.
	/// If this method returns <see cref="AppResult.Failure"/>, <see cref="OnQuit(Sdl, AppResult)"/> will be called and terminate the <see cref="AppBase"/>'s execution with a return value that can be used as an exit code for program that reports an error to the platform.
	/// If it returns <see cref="AppResult.Success"/>, <see cref="OnQuit(Sdl, AppResult)"/> will be called and terminate the <see cref="AppBase"/>'s execution with a return value that can be used as an exit code for the program that reports success to the platform.
	/// </para>
	/// <para>
	/// This method may get called concurrently with <see cref="OnIterate(Sdl)"/> or <see cref="OnQuit(Sdl, AppResult)"/> for events not pushed from the main thread.
	/// </para>
	/// </remarks>
	protected virtual AppResult OnEvent(Sdl sdl, ref readonly Event @event) => Continue;

	/// <summary>
	/// The deinitialising entry point for an <see cref="AppBase"/>'s execution
	/// </summary>
	/// <param name="sdl">The <see cref="Sdl"/> instance that started the execution of this <see cref="AppBase"/></param>
	/// <param name="result">The <see cref="AppResult"/> that terminated the execution of this <see cref="AppBase"/> (<see cref="AppResult.Success"/> or <see cref="AppResult.Failure"/>)</param>
	/// <remarks>
	/// <para>
	/// This method is called once before terminating the execution of the <see cref="AppBase"/>.
	/// </para>
	/// <para>
	/// This method will be called no matter what, even if <see cref="OnInitialize(Sdl, string[])"/> requests termination.
	/// </para>
	/// <para>
	/// This method should not go into an infinite loop; it should deinitialize any resources necessary, perform whatever shutdown activities, and return.
	/// </para>
	/// <para>
	/// You do not need to <see cref="Sdl.Dispose">dispose the <paramref name="sdl"/></see> instance in this method, as that will be done automatically it after this method returns,
	/// but it is safe to do so (with an exception regarding concurrent calls to <see cref="OnEvent(Sdl, ref readonly Event)"/> which might receive the disposed <see cref="Sdl"/> instance by then).
	/// The <paramref name="sdl"/> instance is <see cref="Sdl.Dispose">disposed</see> after the call to this method no matter what. This is unavoidable!
	/// </para>
	/// <para>
	/// This method is called by SDL on the main thread, though <see cref="OnEvent(Sdl, ref readonly Event)"/> may get called concurrently with this method if other threads that push events are still active.
	/// </para>
	/// </remarks>
	protected virtual void OnQuit(Sdl sdl, AppResult result) { }
}
