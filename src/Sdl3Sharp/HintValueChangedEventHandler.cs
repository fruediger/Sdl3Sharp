namespace Sdl3Sharp;

/// <summary>
/// Represents an event that occurs when the value of a <see cref="Hint"/> changes
/// </summary>
/// <param name="sender">The hint which value has changed</param>
/// <param name="oldValue">The previous value of the hint</param>
/// <param name="newValue">The new value the hint has been set to</param>
public delegate void HintValueChangedEventHandler(Hint sender, string? oldValue, string? newValue);