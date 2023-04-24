using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class ElasticEasingFunction : EasingFunctionBase
	{
		[Min(0)] public float sinusoidalFrequency = 3;
		[Min(0)] public float sinusoidalAmplitude = 1;

		protected override float EaseIn01Evaluate(float t)
		{
			float sin = Mathf.Sin(t * (sinusoidalFrequency * Mathf.PI));
			float d = 1 - t;

			float sinusoidalPart = sinusoidalAmplitude * sin * d;
			float exponentialPart = 1 - d * d;

			return sinusoidalPart + exponentialPart;
		}

		public float MaxHeight => 1 + 0.5f * sinusoidalAmplitude;
		public float MinHeight => -0.5f * sinusoidalAmplitude;

		public override Rect EaseIn01ContainingRect => new Rect(
			0,
			MinHeight,
			1,
			MaxHeight - MinHeight);
	}
}