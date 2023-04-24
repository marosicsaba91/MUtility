using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class InfiniteEasingFunction
	{
		[SerializeField, Range(0, 1)] float linearPart = 0.5f;
		[SerializeField, Min(0)] float maximumValue = 1;
		[SerializeField] DisplayMember curvePreview = new DisplayMember(nameof(Evaluate));

		public float LinearPart
		{
			get => linearPart;
			set => linearPart = Mathf.Clamp01(value);
		}

		public float MaximumValue
		{
			get => maximumValue;
			set => maximumValue = Mathf.Max(0, value);
		}

		public InfiniteEasingFunction(float linearPart, float maximumValue)
		{
			this.linearPart = linearPart;
			this.maximumValue = maximumValue;
		}

		public InfiniteEasingFunction() { }

		public float Evaluate(float t)
		{
			if (t <= 0)
				return 0;

			float p = Mathf.Clamp(linearPart * maximumValue, 0, maximumValue);

			if (t <= p)
				return t;

			float h = maximumValue - p;

			return p + (1f - h / (h + t - p)) * h;
		}
	}
}