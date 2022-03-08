using System;
using Random = UnityEngine.Random;

namespace MUtility
{
[Serializable]
public class FloatRange
{
    public float min;
    public float max;
    public float Range => max - min;
    public float GetRandom() => Random.Range(min, max);
    
    public override string ToString() => "(" + this.min.ToString() + "/" + this.max.ToString() + ")";
}
}