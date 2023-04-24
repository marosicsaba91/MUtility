using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	public static class MathHelper
	{
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

		public static float Lerp(float input, float minOutput, float maxOutput, float minInput = 0,
			float maxInput = 1)
		{
			if (input <= minInput)
				return minOutput;
			if (input >= maxInput)
				return maxOutput;
			return minOutput + (input - minInput) / (maxInput - minInput) * (maxOutput - minOutput);
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

		public static int Mod(int n, int m) => (n % m + m) % m;

		public static float Mod(float n, float m) => (n % m + m) % m;

		public static Vector2Int Mod(Vector2Int n, int w, int h) =>
			new Vector2Int(Mod(n.x, w), Mod(n.y, h));

		public static Vector2 Mod(Vector2 n, float w, float h) =>
			new Vector2(Mod(n.x, w), Mod(n.y, h));

		public static Vector3Int Mod(Vector3Int n, int w, int h, int d) =>
			new Vector3Int(Mod(n.x, w), Mod(n.y, h), Mod(n.z, d));

		public static Vector3 Mod(Vector3 n, float w, float h, int d) =>
			new Vector3(Mod(n.x, w), Mod(n.y, h), Mod(n.z, d));


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
	}
}