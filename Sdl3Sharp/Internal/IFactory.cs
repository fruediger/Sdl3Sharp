using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Internal;

internal interface IFactory<T>
{
	[return: NotNull]
	static abstract T Create();
}

internal interface IFactory<TArg, T>
{
	[return: NotNull]
	static abstract T Create(in TArg arg);
}
