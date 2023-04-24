using UnityEngine;

namespace MUtility
{
	public static class ColorExtensions
	{

		public static string ToHex(this Color color) => $"#{(int)(color.r * 255):X2}{(int)(color.g * 255):X2}{(int)(color.b * 255):X2}";
	}
}