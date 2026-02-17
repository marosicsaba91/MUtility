using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MUtility
{
	[Serializable]
	public struct RangedFloat
	{
		public float min;
		public float max;

		public RangedFloat(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
		
		public float Range => max - min;

		public bool Contains(float value) => value >= min && value <= max;
		public float Clamp(float value) => Mathf.Clamp(value, min, max);
		public float Lerp(float t) => Mathf.Lerp(min, max, t);
		public float LerpUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);
		public float InverseLerp(float value) => Mathf.InverseLerp(min, max, value);
		public float InverseLerpUnclamped(float value) => (min == max) ? 0 : (value - min) / (max - min);

		[Obsolete("Use Lerp instead")]
		public float LerpFromRange(float t) => Lerp(t);
		[Obsolete("Use LerpUnclamped instead")]
		public float LerpFromRangeUnclamped(float t) => LerpUnclamped(t);

		public float AbsoluteDistanceOutOfRange(float pitch) =>
			pitch < min ? min - pitch :
			pitch > max ? pitch - max :
			0;

		public float GetRandom() => Random.Range(min, max);

		public float GetRandom(System.Random random)
		{
			float next = (float)random.NextDouble();
			return LerpUnclamped(next);
		}

		public override string ToString() => "(" + min + "/" + max + ")";
	}

	[Serializable]
	public struct RangedInt
	{
		public int min;
		public int max;

		public RangedInt(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
		
		public int Range => max - min;

		public bool Contains(int value) => value >= min && value <= max;
		public bool Contains(float value) => value >= min && value <= max;

		public int Clamp(int value) => Mathf.Clamp(value, min, max);
		public float Clamp(float value) => Mathf.Clamp(value, min, max);

		public float Lerp(float t) => Mathf.Lerp(min, max, t);
		public float LerpUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);

		[Obsolete("Use Lerp instead")]
		public float LerpFromRange(float t) => Lerp(t);
		[Obsolete("Use LerpUnclamped instead")]
		public float LerpFromRangeUnclamped(float t) => LerpUnclamped(t);

		public int AbsoluteDistanceOutOfRange(int pitch) =>
			pitch < min ? min - pitch :
			pitch > max ? pitch - max :
			0;

		public float AbsoluteDistanceOutOfRange(float pitch) =>
			pitch < min ? min - pitch :
			pitch > max ? pitch - max :
			0;

		public int GetRandom() => Random.Range(min, max + 1);

		public override string ToString() => "(" + min + "/" + max + ")";
	}
}