using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal;

internal static class NativeStringHelper
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe static bool Equals(byte* str1, byte* str2)
	{
		if (str1 == str2)
		{
			return true;
		}

		if (str1 is null || str2 is null)
		{
			return false;
		}

		while (true)
		{
			var ch = *str1;

			if (ch != *str2)
			{
				return false;
			}

			if (ch is (byte)'\0')
			{
				return true;
			}

			str1++;
			str2++;
		}
	}
}
