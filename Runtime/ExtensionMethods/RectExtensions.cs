using UnityEngine;

namespace MUtility
{
	public static class RectExtensions
	{
		public static Rect Combine(this Rect self, Rect other)
		{
			float left = Mathf.Min(self.position.x, other.position.x);
			float bottom = Mathf.Min(self.position.y, other.position.y);
			float right = Mathf.Max(self.position.x + self.size.x, other.position.x + other.size.x);
			float top = Mathf.Max(self.position.y + self.size.y, other.position.y + other.size.y);

			return new Rect(left, bottom, right - left, top - bottom);
		}

		public static Rect Crop(this Rect self, Rect crop)
		{
			float left = Mathf.Max(self.xMin, crop.xMin);
			float right = Mathf.Min(self.xMax, crop.xMax);
			float bottom = Mathf.Max(self.yMin, crop.yMin);
			float top = Mathf.Min(self.yMax, crop.yMax);

			return new Rect(left, bottom, right - left, top - bottom);
		}

		public static Rect ChangeLeft(this Rect self, float newValue)
		{
			Rect temp = self;
			temp.xMin = newValue;
			return temp;
		}

		public static Rect ChangeRight(this Rect self, float newValue)
		{
			Rect temp = self;
			temp.xMax = newValue;
			return temp;
		}

		public static Rect ChangeTop(this Rect self, float newValue)
		{
			Rect temp = self;
			temp.yMax = newValue;
			return temp;
		}

		public static Rect ChangeBottom(this Rect self, float newValue)
		{
			Rect temp = self;
			temp.yMin = newValue;
			return temp;
		}


		public static Vector2 TopLeft(this Rect self) => new Vector2(self.xMin, self.yMax);

		public static Vector2 TopRight(this Rect self) => new Vector2(self.xMax, self.yMax);

		public static Vector2 BottomLeft(this Rect self) => new Vector2(self.xMin, self.yMin);

		public static Vector2 BottomRight(this Rect self) => new Vector2(self.xMax, self.yMin);

		public static Vector2 LeftPoint(this Rect self) => new Vector2(self.xMin, self.center.y);

		public static Vector2 RightPoint(this Rect self) => new Vector2(self.xMax, self.center.y);

		public static Vector2 TopPoint(this Rect self) => new Vector2(self.center.x, self.yMax);

		public static Vector2 BottomPoint(this Rect self) => new Vector2(self.center.x, self.yMin);
	}
}

