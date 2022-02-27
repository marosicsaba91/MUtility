using System;
using Random = UnityEngine.Random;

namespace MUtility
{
[Serializable]
public class IntRange
{
    public int min;
    public int max;
    public int Range => max - min;
    public int GetRandom() => Random.Range(min, max);
}
}