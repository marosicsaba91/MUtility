using System;
using System.Collections.Generic;

namespace MUtility
{
	public static class ListExtensions
	{
		public static void Add<T>(this List<T> list, T value, int length)
		{
			for (int i = 0; i < length; i++)
				list.Add(value);
		}

		public static void Add<T>(this List<T> array, T startValue, Func<T, T> next, int length)
		{
			if (length <= 0)
				return;

			T last = startValue;
			array.Add(startValue);
			for (int i = 1; i < length; i++)
			{
				last = next(last);
				array.Add(last);
			}
		}

		public static void CopyTo<T>(this List<T> source, List<T> destination) => source.CopyTo(destination, source.Count);

		public static void CopyTo<T>(this List<T> source, List<T> destination, int length) =>
			source.CopyTo(0, destination, 0, length);

		public static void CopyTo<T>(this List<T> source, int sourceIndex, List<T> destination, int destinationIndex, int length)
		{
			int destinationCount = destination.Count;

			int di = destinationIndex;
			for (int si = sourceIndex; si < length; si++)
			{
				if (destinationCount >= di)
					destination.Add(source[si]);
				else
					destination[di] = source[si];
				di++;
			}
		}

		public static void SetCount<T>(this List<T> list, int count, Func<T> create)
		{
			if (list.Count > count)
				list.RemoveRange(count, list.Count - count);
			else
			{
				while (list.Count < count)
					list.Add(create());
			}
		}
	}
}
