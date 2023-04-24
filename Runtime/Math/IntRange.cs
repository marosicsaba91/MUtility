using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MUtility
{
	[Serializable]
	public struct IntRange
	{
		public int min;
		public int max;
		public int Range => max - min;
		public int GetRandom() => Random.Range(min, max);
		public float Lerp(float t) => Mathf.Lerp(min, max, t);
		public float LerpUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);
		public int Clamp(int value) => Mathf.Clamp(value, min, max);
		public float Clamp(float value) => Mathf.Clamp(value, min, max);

		public override string ToString() => "(" + min.ToString() + "/" + max.ToString() + ")";
	}
}