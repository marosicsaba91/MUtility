using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public abstract class EasingFunctionBase
	{
		public enum Increasing
		{
			Up,
			Down
		}

		public enum Smooth
		{
			StartSmooth,
			EndSmooth,
			FullSmooth
		}


		public Increasing increasing = Increasing.Up;
		public Smooth smooth = Smooth.FullSmooth;
		[SerializeField] DisplayMember curvePreview = new DisplayMember(nameof(Evaluate));

		public Rect DefaultArea
		{
			get
			{
				Rect easeIn01Rect = EaseIn01ContainingRect;
				float maxH =
					smooth == Smooth.EndSmooth ? easeIn01Rect.yMax :
					smooth == Smooth.StartSmooth ? 1 - easeIn01Rect.yMin :
					1 - (1 - easeIn01Rect.yMax) / 2;
				float minH =
					smooth == Smooth.EndSmooth ? easeIn01Rect.yMin :
					smooth == Smooth.StartSmooth ? 1 - easeIn01Rect.yMax :
					(1 - easeIn01Rect.yMax) / 2;

				if (increasing == Increasing.Down)
				{
					float temp = minH;
					minH = 1 - maxH;
					maxH = 1 - temp;
				}

				return new Rect(0, minH, 1, maxH - minH);
			}
		}

		public float Evaluate(float time)
		{
			if (time <= 0)
				return increasing == Increasing.Up ? 0 : 1;
			if (time >= 1)
				return increasing == Increasing.Up ? 1 : 0;

			float result;

			if (smooth == Smooth.EndSmooth)
			{
				result = EaseIn01Evaluate(time);
			}
			else if (smooth == Smooth.StartSmooth)
			{
				result = 1 - EaseIn01Evaluate(1 - time);
			}
			else
			{
				if (time > 0.5f)
				{
					time = (time - 0.5f) * 2;
					result = EaseIn01Evaluate(time);
					result /= 2f;
					result += 0.5f;
				}
				else
				{
					time = (0.5f - time) * 2;
					result = 1 - EaseIn01Evaluate(time);
					result /= 2f;
				}
			}

			if (increasing == Increasing.Down)
				return 1 - result;

			return result;
		}

		protected abstract float EaseIn01Evaluate(float time);

		public virtual Rect EaseIn01ContainingRect => new Rect(0, 0, 1, 1);
	}
}