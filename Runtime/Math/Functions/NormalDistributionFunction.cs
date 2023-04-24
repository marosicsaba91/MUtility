using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MUtility
{
	[Serializable]
	public class NormalDistributionFunction
	{
		public float mean = 0;
		[SerializeField, Min(0)] float stdDev = 1;
		[SerializeField] DisplayMember curvePreview = new DisplayMember(nameof(Evaluate));

		public float StdDev
		{
			get => stdDev;
			set => Mathf.Max(0, value);
		}

		public float GetRandom() => GetRandom(mean, stdDev);

		public static float GetRandom(float mean, float stdDev)
		{
			float u1 = Random.Range(0f, 1f);
			float u2 = Random.Range(0f, 1f);
			if (u1 == 0 || u2 == 0)
			{
				return 0;
			}

			float randStdNormal =
				Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
				Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
			return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
		}

		public float Evaluate(float input) => Evaluate(input, mean, stdDev);

		const float sqrt2PIInvert = 0.398942280401432677939946059934381868f; // 1f / Mathf.Sqrt(2 * Mathf.PI);

		public float MaxY => sqrt2PIInvert / stdDev;

		public static float Evaluate(float input, float mean, float stdDev)
		{
			const float sqrt2PI = 2.506628274631000502415765284811f; // Mathf.Sqrt(2 * Mathf.PI);
			const float e = 2.71828182845904523536028747135f;

			float n = (input - mean) / stdDev;
			float p = -0.5f * n * n;

			return Mathf.Pow(e, p) / (stdDev * sqrt2PI);
		}
	}
}