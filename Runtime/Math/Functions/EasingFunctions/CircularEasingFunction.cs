using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class CircularEasingFunction : EasingFunctionBase
	{
		protected override float EaseIn01Evaluate(float t) =>
			Mathf.Sqrt(1 - (1 - t) * (1 - t));

	}
}