namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a method that is used to comparer two item of type <typeparamref name="T"/> regarding their ordering
/// </summary>
/// <typeparam name="T">The type of items to compare</typeparam>
/// <param name="a">The first item to compare</param>
/// <param name="b">The second item to compaer</param>
/// <returns>
/// <c>-1</c>, if <paramref name="a"/> should be ordered <em>before</em> <paramref name="b"/>;
/// <c>1</c>, if <paramref name="a"/> should be ordered <em>after</em> <paramref name="b"/>;
/// otherwise, <c>0</c> and <paramref name="a"/> and <paramref name="b"/> are consindered equal
/// </returns>
/// <remarks>
/// <para>
/// If the two items <paramref name="a"/> and <paramref name="b"/> are equal (the return value of this method is <c>0</c>), their order while sorting is undefined.
/// </para>
/// </remarks>
public delegate int Comparer<T>(ref readonly T a, ref readonly T b) where T : unmanaged;