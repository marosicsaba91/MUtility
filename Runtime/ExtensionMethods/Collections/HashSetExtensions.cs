using System;
using System.Collections.Generic;

namespace MUtility
{
	public static class HashSetExtensions
	{
		public static void CopyTo<T>(this HashSet<T> source, HashSet<T> destination)
		{
			foreach (T element in source)
				destination.Add(element);
		}

		public static List<T> Where<T>(this HashSet<T> hashSet, Func<T, bool> predicate, List<T> output)
		{
			output.Clear();
			int listCount = hashSet.Count;
			foreach (T element in hashSet)
				if (predicate(element))
					output.Add(element);
			return output;
		}

		public static List<TResult> Select<TSource, TResult>(this HashSet<TSource> hashSet, Func<TSource, TResult> selector, List<TResult> output)
		{
			output.Clear();
			int listCount = hashSet.Count;
			foreach (TSource element in hashSet)
				output.Add(selector(element));
			return output;
		}

		public static int Sum(this HashSet<int> source)
		{
			Func<int, int, int> func = (accumulator, newElement) => accumulator + newElement;
			return source.Aggregate(func);
		}

		public static float Sum(this HashSet<float> source)
		{
			Func<float, float, float> func = (accumulator, newElement) => accumulator + newElement;
			return source.Aggregate(func);
		}

		public static int Min(this HashSet<int> source)
		{
			Func<int, int, int> func = (accumulator, newElement) => accumulator.CompareTo(newElement) < 0 ? accumulator : newElement;
			return source.Aggregate(func);
		}

		public static float Min(this HashSet<float> source)
		{
			Func<float, float, float> func = (accumulator, newElement) => accumulator.CompareTo(newElement) < 0 ? accumulator : newElement;
			return source.Aggregate(func);
		}

		public static int Max(this HashSet<int> source)
		{
			Func<int, int, int> func = (accumulator, newElement) => accumulator > newElement ? accumulator : newElement;
			return source.Aggregate(func);
		}

		public static float Max(this HashSet<float> source)
		{
			Func<float, float, float> func = (accumulator, newElement) => accumulator > newElement ? accumulator : newElement;
			return source.Aggregate(func);
		}

		public static float Average(this HashSet<int> source) => (float)source.Sum() / source.Count;

		public static float Average(this HashSet<float> source) => source.Sum() / source.Count;

		public static TAccumulate Aggregate<TSource, TAccumulate>(this HashSet<TSource> hashSet, Func<TAccumulate, TSource, TAccumulate> func)
		{
			TAccumulate accumulator = default;
			foreach (TSource element in hashSet)
				accumulator = func(accumulator, element);
			return accumulator;
		}

		public static T GetRandomElement<T>(this HashSet<T> hashSet)
		{
			int randomIndex = UnityEngine.Random.Range(0, hashSet.Count);
			int index = 0;
			foreach (T item in hashSet)
			{
				if (index == randomIndex)
					return item;
				index++;
			}
			throw new IndexOutOfRangeException("HashSet is Empty!");
		}

		public static bool TryGetRandomElement<T>(this HashSet<T> hashSet, Predicate<T> predicate, out T result)
		{
			int count = hashSet.Count;

			int elementCountWithPredicate = 0;
			foreach (T element in hashSet)
				if (predicate(element))
				{
					elementCountWithPredicate++;
				}


			if (elementCountWithPredicate == 0)
			{
				result = default;
				return false;
			}

			int random = UnityEngine.Random.Range(0, elementCountWithPredicate);
			int index = 0;
			foreach (T element in hashSet)
				if (predicate(element))
				{
					if (index == random)
					{
						result = element;
						return true;
					}
					index++;
				}

			throw new Exception("Unreachable Code!");
		}
	}
}
