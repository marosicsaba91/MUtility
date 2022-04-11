using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MUtility
{
[Serializable]
public struct FloatRange
{
    public float min;
    public float max;
    public float Range => max - min;
	
    public float GetRandom() => Random.Range(min, max);	
	public float Lerp(float t) => Mathf.Lerp(min, max, t);
    public float LerpUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);
    
    public override string ToString() => "(" + this.min.ToString() + "/" + this.max.ToString() + ")";
}
}