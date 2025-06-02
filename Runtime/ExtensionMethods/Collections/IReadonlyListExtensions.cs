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

		public static bool Contains<T>(this IReadOnlyList<T> stack, T item)
		{
			if (stack == null || stack.Count == 0)
				return false;
			for (int i = 0; i < stack.Count; i++)
				if (stack[i].Equals(item))
					return true;
			return false;
		}
	}
}
