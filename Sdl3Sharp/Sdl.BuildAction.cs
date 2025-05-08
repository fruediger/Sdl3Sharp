namespace Sdl3Sharp;

partial class Sdl
{
	/// <summary>
	/// Represents an action that is performed right before initializing SDL. Use the provided <paramref name="builder"/> argument to perfom some preliminaries before SDL gets initialized.
	/// </summary>
	/// <param name="builder">A <see cref="Builder"/> that lets you perfom some preliminaries right before SDL gets initialized</param>
	public delegate void BuildAction(Builder builder);
}
