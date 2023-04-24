using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class ExponentialEasingFunction : EasingFunctionBase
	{
		[Min(1)] public float exponent = 2;

		protected override float EaseIn01Evaluate(float t) =>
			1 - Mathf.Pow(1 - t, exponent);
	}
}