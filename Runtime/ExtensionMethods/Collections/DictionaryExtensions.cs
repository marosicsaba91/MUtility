using System;
using System.Collections.Generic;

namespace MUtility
{
	public static class DictionaryExtensions
	{
		public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> source)
			=> source == null || source.Count == 0;

		public static bool NotNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> source)
			=> !source.IsNullOrEmpty();

		public delegate bool Predicate<in TKey, in TValue>(TKey key, TValue value);

		public static void CopyTo<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> destination)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in source)
				destination.Add(kvp.Key, kvp.Value);
		}

		public static bool TryGetKeyOf<TKey, TValue>(this Dictionary<TKey, TValue> source, TValue value, out TKey key)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in source)
				if (kvp.Value.Equals(value))
				{
					key = kvp.Key;
					return true;
				}
			key = default;
			return false;
		}

		public static Dictionary<TKey, TValue> Where<TKey, TValue>(this Dictionary<TKey, TValue> dict, Predicate<TKey, TValue> predicate, Dictionary<TKey, TValue> output)
		{
			output.Clear();
			foreach (KeyValuePair<TKey, TValue> element in dict)
			{
				if (predicate(element.Key, element.Value))
				{
					output.Add(element.Key, element.Value);
				}
			}
			return output;
		}

		public static List<TResult> Select<TKey, TValue, TResult>(this Dictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, TResult> selector, List<TResult> output)
		{
			output.Clear();
			foreach (KeyValuePair<TKey, TValue> element in dict)
			{
				output.Add(selector(element));
			}
			return output;
		}

		public delegate TAccumulate DictionaryAggregateDelegate
			<TAccumulate, in TKey, in TValue>
			(TAccumulate accumulator, TKey key, TValue value);

		public static TAccumulate Aggregate<TAccumulate, TKey, TValue>(
			this Dictionary<TKey, TValue> dict,
			DictionaryAggregateDelegate<TAccumulate, TKey, TValue> func,
			TAccumulate accumulatorStartValue = default)
		{
			foreach (KeyValuePair<TKey, TValue> element in dict)
				accumulatorStartValue = func(accumulatorStartValue, element.Key, element.Value);

			return accumulatorStartValue;
		}

		public static TKey MinKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey maxPossibleValue)
			where TKey : IComparable =>
			dictionary.Aggregate(
				(accumulator, key, value) => key.CompareTo(accumulator) == -1 ? key : accumulator,
				maxPossibleValue);

		public static TKey MaxKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey minPossibleValue)
			where TKey : IComparable =>
			dictionary.Aggregate(
				(accumulator, key, value) => key.CompareTo(accumulator) == 1 ? key : accumulator,
				minPossibleValue);

		public static TValue MinValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue maxPossibleValue)
			where TValue : IComparable =>
			dictionary.Aggregate(
				(accumulator, key, value) => value.CompareTo(accumulator) == -1 ? value : accumulator,
				maxPossibleValue);

		public static TValue MaxValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue minPossibleValue)
			where TValue : IComparable =>
			dictionary.Aggregate(
				(accumulator, key, value) => value.CompareTo(accumulator) == 1 ? value : accumulator,
				minPossibleValue);



		public static Dictionary<TKey, TValue> GetSortedByKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : IComparable =>
			dictionary.GetSortedByKeys((a, b) => a.CompareTo(b));

		public static Dictionary<TKey, TValue> GetSortedByValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TValue : IComparable =>
			dictionary.GetSortedByValues((a, b) => a.CompareTo(b));

		public static Dictionary<TKey, TValue> GetSortedByKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Comparison<TKey> comparison) =>
			dictionary.GetSorted((a, b) => comparison(a.Key, b.Key));

		public static Dictionary<TKey, TValue> GetSortedByValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Comparison<TValue> comparison) =>
			dictionary.GetSorted((a, b) => comparison(a.Value, b.Value));

		public static Dictionary<TKey, TValue> GetSorted<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Comparison<KeyValuePair<TKey, TValue>> comparison)
		{
			var result = new Dictionary<TKey, TValue>();
			var sorted = new List<KeyValuePair<TKey, TValue>>();

			foreach (KeyValuePair<TKey, TValue> item in dictionary)
				sorted.Add(item);

			sorted.Sort(comparison);

			foreach (KeyValuePair<TKey, TValue> item in sorted)
				result.Add(item.Key, item.Value);

			return result;
		}

		public static bool IsSortedByKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : IComparable =>
			dictionary.IsSortedByKeys((a, b) => a.CompareTo(b));

		public static bool IsSortedByValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TValue : IComparable =>
			dictionary.IsSortedByValues((a, b) => a.CompareTo(b));

		public static bool IsSortedByKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Comparison<TKey> comparison) =>
			dictionary.IsSorted((a, b) => comparison(a.Key, b.Key));

		public static bool IsSortedByValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Comparison<TValue> comparison) =>
			dictionary.IsSorted((a, b) => comparison(a.Value, b.Value));


		public static bool IsSorted<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Comparison<KeyValuePair<TKey, TValue>> comparison)
		{
			KeyValuePair<TKey, TValue>? last = null;
			foreach (KeyValuePair<TKey, TValue> element in dictionary)
			{
				if (last != null && comparison(last.Value, element) > 0)
					return false;
				last = element;
			}

			return true;
		}

		public static KeyValuePair<TKey, TValue> GetRandomElement<TKey, TValue>(this Dictionary<TKey, TValue> dict)
		{
			int randomIndex = UnityEngine.Random.Range(0, dict.Count);
			int index = 0;
			foreach (KeyValuePair<TKey, TValue> item in dict)
			{
				if (index == randomIndex)
					return item;
				index++;
			}
			throw new IndexOutOfRangeException("Dictionary is Empty!");
		}

		public static bool TryGetRandomElement<TKey, TValue>(
			this Dictionary<TKey, TValue> dict,
			Predicate<TKey, TValue> predicate,
			out KeyValuePair<TKey, TValue> result)
		{

			int elementCountWithPredicate = 0;
			foreach (KeyValuePair<TKey, TValue> element in dict)
			{
				if (predicate(element.Key, element.Value))
				{
					elementCountWithPredicate++;
				}
			}

			if (elementCountWithPredicate == 0)
			{
				result = default;
				return false;
			}

			int randomIndex = UnityEngine.Random.Range(0, elementCountWithPredicate);
			int index = 0;
			foreach (KeyValuePair<TKey, TValue> element in dict)
			{
				if (!predicate(element.Key, element.Value))
					continue;

				if (index == randomIndex)
				{
					result = element;
					return true;
				}
				index++;
			}

			throw new Exception("Unreachable Code!");
		}

		public static void AddOrChangeValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary.ContainsKey(key))
				dictionary[key] = value;
			else
				dictionary.Add(key, value);
		}
	}
}
