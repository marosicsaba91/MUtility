using System.Linq;
using UnityEngine;

namespace MUtility
{
	public static class ColorHelper
	{
		public static Color GradientLerp(float t, params Color[] colors)
		{
			if (colors == null)
				return Color.black;
			int colorsCount = colors.Length;
			if (colorsCount == 0)
				return Color.black;
			if (colorsCount == 1)
				return colors.First();
			if (t <= 0)
				return colors.First();
			if (t >= 1)
				return colors.Last();
			int i1 = (int)((colorsCount - 1) * t);
			int i2 = i1 + 1;
			float f = 1f / (colorsCount - 1);
			float innerT = (t - i1 * f) / f;
			return Color.Lerp(colors[i1], colors[i2], innerT);
		}
	}
}