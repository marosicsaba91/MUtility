using System;
using System.Collections.Generic;

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
        int destCount = destination.Count;

        int j = 0;
        for (int i = 0; i < length; i++)
        {
            if (destCount >= j)
            {
                destination.Add(source[i]);
            }
            else
            {
                destination[i] = source[j];
            }
            j++;
        }
    }
}
