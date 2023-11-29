using System.Collections.Generic;

namespace MUtility
{
	public static class IReadonlyListExtensions
	{
		public static T IndexClamped<T>(this IReadOnlyList<T> source, int index)
		{
			if (source.Count == 0) return default;
			if (source.Count <= index) return source[^1];
			if (index < 0) return source[0];
			return source[index];
		}
	}
}
