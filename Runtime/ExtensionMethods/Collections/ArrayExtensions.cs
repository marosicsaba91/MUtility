using System;

namespace MUtility
{
	public static class ArrayExtensions
	{
		public static bool IsNullOrEmpty<T>(this T[] array)
			=> array == null || array.Length == 0;

		public static bool NotNullOrEmpty<T>(this T[] array)
			=> !IsNullOrEmpty(array);

		public static void Fill<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
				array[i] = value;
		}

		public static void Fill<T>(this T[] array, T startValue, Func<T, T> next)
		{
			if (array.Length == 0)
				return;

			array[0] = startValue;
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = next(array[i - 1]);
			}
		}

		public static T[] CreateCopy<T>(this T[] original)
		{
			if (original == null)
				return null;
			var result = new T[original.Length];
			CopyTo(original, result);
			return result;
		}

		public static void CopyTo<T>(this T[] source, T[] destination) =>
			Array.Copy(source, destination, Math.Min(source.Length, destination.Length));

		public static void CopyTo<T>(this T[] source, T[] destination, int length) =>
			Array.Copy(source, destination, length);

		public static void CopyTo<T>(this T[] source, int sourceIndex, T[] destination, int destinationIndex, int length) =>
			Array.Copy(source, sourceIndex, destination, destinationIndex, length);

		public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf(array, value);

		public static bool Contains<T>(this T[] array, T value) => Array.IndexOf(array, value) > -1;

		public static T[] SubArray<T>(this T[] data, int index, int length)
		{
			if (data.Length <= 0 ||
				index >= data.Length ||
				length + index > data.Length ||
				length <= 0 ||
				index < 0)
				return null;

			var result = new T[length];
			Array.Copy(data, index, result, 0, length);
			return result;
		}
	}
}
