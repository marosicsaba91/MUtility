using System;
using System.Collections.Generic;
using System.Linq;

namespace MUtility
{
	public static class IListExtensions
	{
		public static bool IsNullOrEmpty<T>(IList<T> list)
			=> list == null || list.Count == 0;

		public static bool NotNullOrEmpty<T>(IList<T> list)
			=> !IsNullOrEmpty(list);

		public static float Average(this IList<int> source) => (float)source.Sum() / source.Count;

		public static float Average(this IList<float> source) => source.Sum() / source.Count;

		public static bool ContainsSubset<T>(this IList<T> whole, IList<T> subset)
		{
			for (int i = 0; i < subset.Count; i++)
				if (!whole.Contains(subset[i]))
					return false;
			return true;
		}

		public static IList<T> Shuffle<T>(this IList<T> list)
		{
			if (list == null || list.Count == 0)
				return list;

			for (int i = 0; i < list.Count - 1; i++)
			{
				int index = UnityEngine.Random.Range(i, list.Count);
				list.Swap(i, index);
			}
			return list;
		}

		public static void Swap<T>(this IList<T> list, int i, int j) => (list[i], list[j]) = (list[j], list[i]);

		public static T GetRandomElement<T>(this IList<T> source) =>
			source[UnityEngine.Random.Range(0, source.Count)];

		public static bool TryGetRandomElement<T>(this IList<T> source, Predicate<T> predicate, out T result)
		{
			int count = source.Count;

			int elementCountWithPredicate = 0;
			for (int i = 0; i < count; i++)
			{
				T element = source[i];
				if (predicate(element))
				{
					elementCountWithPredicate++;
				}
			}

			if (elementCountWithPredicate == 0)
			{
				result = default;
				return false;
			}

			int random = UnityEngine.Random.Range(0, elementCountWithPredicate);
			int index = 0;
			for (int i = 0; i < count; i++)
			{
				T element = source[i];
				if (predicate(element))
				{
					if (index == random)
					{
						result = element;
						return true;
					}
					index++;
				}
			}

			throw new Exception("Unreachable Code!");
		}
	}
}
