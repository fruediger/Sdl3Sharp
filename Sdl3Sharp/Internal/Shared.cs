using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sdl3Sharp.Internal;

internal static class Shared
{
	internal sealed class StringBuilderFactory : IFactory<StringBuilder>
	{
		[return: NotNull]
		public static StringBuilder Create() => new();
	}

	internal static readonly SimpleConcurrentPool<StringBuilder, StringBuilderFactory> StringBuilderPool = new();
}
