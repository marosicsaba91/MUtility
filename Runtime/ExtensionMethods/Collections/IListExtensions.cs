using System;
using System.Collections.Generic;

namespace MUtility
{
public static class IListExtensions {

    public static T First<T>(this IList<T> list) => list[0];

    public static T Last<T>(this IList<T> list) => list[list.Count - 1];

    public static void Where<T>(this IList<T> source, Func<T, bool> predicate, List<T> output)
    {
        output.Clear();
        int listCount = source.Count;
        for (var i = 0; i < listCount; i++)
        {
            var element = source[i];
            if (predicate(element))
            {
                output.Add(element);
            }
        } 
    }

    public static void Select<TSource, TResult>(this IList<TSource> source, Func<TSource, TResult> selector, List<TResult> output)
    {
        output.Clear();
        int listCount = source.Count;
        for (var i = 0; i < listCount; i++)
        {
            var element = source[i];
            output.Add(selector(element));
        } 
    }

    public static int Sum(this IList<int> source)
    {
        Func<int, int, int> func = (accumulator, newElement) => accumulator + newElement;
        return source.Aggregate(func);
    }

    public static float Sum(this IList<float> source)
    {
        Func<float, float, float> func = (accumulator, newElement) => accumulator + newElement;
        return source.Aggregate(func);
    }

    public static int Min(this IList<int> source)
    {
        Func<int, int, int> func = (accumulator, newElement) => accumulator.CompareTo(newElement) < 0 ? accumulator : newElement;
        return source.Aggregate(func);
    }

    public static float Min(this IList<float> source)
    {
        Func<float, float, float> func = (accumulator, newElement) => accumulator.CompareTo(newElement) < 0 ? accumulator : newElement;
        return source.Aggregate(func);
    }

    public static int Max(this IList<int> source)
    {
        Func<int, int, int> func = (accumulator, newElement) => accumulator > newElement ? accumulator : newElement;
        return source.Aggregate(func);
    }

    public static float Max(this IList<float> source)
    {
        Func<float, float, float> func = (accumulator, newElement) => accumulator > newElement ? accumulator : newElement;
        return source.Aggregate(func);
    }

    public static float Average(this IList<int> source) => ((float)source.Sum()) / source.Count;

    public static float Average(this IList<float> source) => source.Sum() / source.Count;

    public static TAccumulate Aggregate<TSource, TAccumulate>(this IList<TSource> source, Func<TAccumulate, TSource, TAccumulate> func, TAccumulate accumulator = default)
    { 
        for (var i = 0; i < source.Count; i++)
        {
            TSource element = source[i];
            accumulator = func(accumulator, element);
        }
        return accumulator;
    }

    public static bool ContainsSubset<T>(this IList<T> whole, IList<T> subset)
    {
        for (var i = 0; i < subset.Count; i++)
            if (!whole.Contains(subset[i]))
                return false;
        return true;
    }
    
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
            return list;

        for (var i = 0; i < list.Count - 1; i++)
        {
            int index = UnityEngine.Random.Range(i, list.Count);
            list.Swap(i, index);
        }
        return list;
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    public static T GetRandomElement<T>(this IList<T> source) =>    
        source[UnityEngine.Random.Range(0, source.Count)];

    public static bool TryGetRandomElement<T>(this IList<T> source, Predicate<T> predicate, out T result)
    {
        int count = source.Count;

        var elementCountWithPredicate = 0;
        for (var i = 0; i < count; i++)
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
        var index = 0;
        for (var i = 0; i < count; i++)
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
