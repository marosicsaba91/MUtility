using UnityEngine;

namespace MUtility
{
	public static class RandomExtensions
	{
		public static Vector2 InsideUnitCircle(this System.Random random)
		{
			float angle = (float)(random.NextDouble() * Mathf.PI * 2);
			float radius = (float)(random.NextDouble());
			radius = Mathf.Sqrt(radius); // To ensure uniform distribution within the circle
			return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
		}
		public static Vector2 OnUnitCircle(this System.Random random)
		{
			float angle = (float)(random.NextDouble() * Mathf.PI * 2);
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}
