using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	public static class MathHelper
	{
		public static float Lerp(float input, float minOutput, float maxOutput, float minInput = 0,
			float maxInput = 1)
		{
			if (input <= minInput)
				return minOutput;
			if (input >= maxInput)
				return maxOutput;
			return minOutput + (input - minInput) / (maxInput - minInput) * (maxOutput - minOutput);
		}

		public static float LerpUnclamped(float input, float minOutput, float maxOutput,
			float minInput = 0, float maxInput = 1)
		{
			return minOutput + (input - minInput) / (maxInput - minInput) * (maxOutput - minOutput);
		}

		public static float Towards(float current, float target, float maxDelta)
		{
			if (Mathf.Abs(target - current) > float.Epsilon)
			{
				float difference = target - current;
				if (maxDelta >= Mathf.Abs(difference))
					current = target;
				else
					current += maxDelta * (target > current ? 1 : -1);
			}

			return current;
		}

		public static Vector2 LerpUnclamped(float input, Vector2 minOutput, Vector2 maxOutput,
			float minInput = 0, float maxInput = 1)
		{
			float outX = minOutput.x +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.x - minOutput.x);
			float outY = minOutput.y +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.y - minOutput.y);
			return new Vector2(outX, outY);
		}

		public static Vector2 Lerp(float input, Vector2 minOutput, Vector2 maxOutput,
			float minInput = 0, float maxInput = 1)
		{
			if (input < minInput)
				return minOutput;
			if (input > maxInput)
				return maxOutput;
			float outX = minOutput.x +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.x - minOutput.x);
			float outY = minOutput.y +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.y - minOutput.y);
			return new Vector2(outX, outY);
		}

		public static Vector3 LerpUnclamped(float input, Vector3 minOutput, Vector3 maxOutput,
			float minInput = 0, float maxInput = 1)
		{
			float outX = minOutput.x +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.x - minOutput.x);
			float outY = minOutput.y +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.y - minOutput.y);
			float outZ = minOutput.z +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.z - minOutput.z);
			return new Vector3(outX, outY, outZ);
		}

		public static Vector3 Lerp(float input, Vector3 minOutput, Vector3 maxOutput,
			float minInput = 0, float maxInput = 1)
		{
			if (input < minInput)
				return minOutput;
			if (input > maxInput)
				return maxOutput;
			float outX = minOutput.x +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.x - minOutput.x);
			float outY = minOutput.y +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.y - minOutput.y);
			float outZ = minOutput.z +
						 (input - minInput) / (maxInput - minInput) * (maxOutput.z - minOutput.z);
			return new Vector3(outX, outY, outZ);
		}

		// This modulo function is different from the % operator in C#.
		// It always returns a positive number.
		public static int ModuloPositive(int n, int m) => (n % m + m) % m;

		public static float ModuloPositive(float n, float m) => (n % m + m) % m;

		public static Vector2Int ModuloPositive(Vector2Int n, int w, int h) =>
			new(ModuloPositive(n.x, w), ModuloPositive(n.y, h));

		public static Vector2 ModuloPositive(Vector2 n, float w, float h) =>
			new(ModuloPositive(n.x, w), ModuloPositive(n.y, h));

		public static Vector3Int ModuloPositive(Vector3Int n, int w, int h, int d) =>
			new(ModuloPositive(n.x, w), ModuloPositive(n.y, h), ModuloPositive(n.z, d));

		public static Vector3 ModuloPositive(Vector3 n, float w, float h, int d) =>
			new(ModuloPositive(n.x, w), ModuloPositive(n.y, h), ModuloPositive(n.z, d));


		// Code duplication is for optimisation purposes.
		// Special Aggregate Functions : Float 

		public static float Average(params float[] values) => Average((IList<float>)values);
		public static float Average(IList<float> values) => Sum(values) / values.Count;

		public static float Sum(params float[] values) => Sum((IList<float>)values);
		public static float Sum(IList<float> values)
		{
			float accumulator = 0;
			for (int i = 0; i < values.Count; i++)
				accumulator += values[i];
			return accumulator;
		}

		// Special Aggregate Functions : Vector2 

		public static Vector2 Average(params Vector2[] values)
			=> Average((IList<Vector2>)values);
		public static Vector2 Average(IList<Vector2> values) => Sum(values) / values.Count;

		public static Vector2 Sum(params Vector2[] values) => Sum((IList<Vector2>)values);
		public static Vector2 Sum(IList<Vector2> values)
		{
			Vector2 accumulator = Vector2.zero;
			for (int i = 0; i < values.Count; i++)
			{
				Vector2 element = values[i];
				accumulator.x += element.x;
				accumulator.y += element.y;
			}

			return accumulator;
		}

		// Special Aggregate Functions : Vector3

		public static Vector3 Average(params Vector3[] values)
			=> Average((IList<Vector3>)values);

		public static Vector3 Sum(params Vector3[] values) => Sum((IList<Vector3>)values);
		public static Vector3 Average(IList<Vector3> values) => Sum(values) / values.Count;
		public static Vector3 Sum(IList<Vector3> values)
		{
			Vector3 accumulator = Vector3.zero;
			for (int i = 0; i < values.Count; i++)
			{
				Vector3 element = values[i];
				accumulator.x += element.x;
				accumulator.y += element.y;
				accumulator.z += element.z;
			}

			return accumulator;
		}

		// Probability
		public static bool ChanceInPercent(int percent) => Random.Range(0, 100) < percent;

		public static bool Chance(float rate) => Random.Range(0f, 1f) < rate;

		// Rounding to any steps 

		public static float Round(float value, float step, float maxDistance = 0)
		{
			if (maxDistance >= step / 2 || maxDistance <= 0)
				return Mathf.Round(value / step) * step;

			float mod = ModuloPositive(value, step);
			if (mod < maxDistance)
				value -= mod;
			else if (mod > step - maxDistance)
				value += step - mod;

			return value;
		}

		public static float SmoothBetween01(float t, float sharpness, bool hardStart = false, bool hardEnd = false)
		{
			if (t <= 0)
				return 0;
			if (t >= 1)
				return 1;

			if (hardStart && hardEnd)
				return t;

			// Transform t between -1 and 1
			float cut;
			if (hardEnd)
				cut = t - 1;
			else if (hardStart)
				cut = t;
			else
				cut = (t * 2) - 1;

			float sin = Mathf.Sin(cut * Mathf.PI / 2);

			// Apply sharpness
			float curved;
			if (sharpness >= 1)
				curved = Mathf.Sign(sin) * Mathf.Pow(Mathf.Abs(sin), 1 / sharpness);
			else
				curved = Mathf.Lerp(cut, sin, sharpness);

			// Smooth Between 0-1
			if (hardEnd)
				return curved + 1;
			else if (hardStart)
				return curved;
			else
				return 0.5f + curved / 2;
		}

		public static float InverseSmooth(float smoothened, float sharpness, bool hardStart = false, bool hardEnd = false)
		{
			if (smoothened <= 0)
				return 0;
			if (smoothened >= 1)
				return 1;
			if (hardStart && hardEnd)
				return smoothened;

			// Smooth Between 0-1
			float curved;
			if (hardEnd)
				curved = smoothened - 1;
			else if (hardStart)
				curved = smoothened;
			else
				curved = (smoothened * 2) - 1;

			// Apply sharpness
			float sin;
 			if (sharpness >= 1)
				sin = Mathf.Sign(curved) * Mathf.Pow(Mathf.Abs(curved), sharpness);
			else
				sin = Mathf.Lerp(curved, Mathf.Sin(curved * Mathf.PI / 2), sharpness);

			float cut = Mathf.Asin(sin) * 2 / Mathf.PI;

			// Transform smoothened between -1 and 1 
			if (hardEnd)
				return cut + 1;
			else if (hardStart)
				return cut;
			else
				return (cut + 1) / 2;

		}
	}
}