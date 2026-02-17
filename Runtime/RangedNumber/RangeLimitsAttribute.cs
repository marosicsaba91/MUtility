using UnityEngine;

namespace MUtility
{
	public class RangeLimitsAttribute : PropertyAttribute
	{
		public readonly float min;
		public readonly float max;

		public RangeLimitsAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}