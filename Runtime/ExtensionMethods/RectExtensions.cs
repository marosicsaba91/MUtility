using System.Reflection;
using UnityEditor;
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


		public static Vector2 TopLeft(this Rect self) => new(self.xMin, self.yMax);

		public static Vector2 TopRight(this Rect self) => new(self.xMax, self.yMax);

		public static Vector2 BottomLeft(this Rect self) => new(self.xMin, self.yMin);

		public static Vector2 BottomRight(this Rect self) => new(self.xMax, self.yMin);

		public static Vector2 LeftPoint(this Rect self) => new(self.xMin, self.center.y);

		public static Vector2 RightPoint(this Rect self) => new(self.xMax, self.center.y);

		public static Vector2 TopPoint(this Rect self) => new(self.center.x, self.yMax);

		public static Vector2 BottomPoint(this Rect self) => new(self.center.x, self.yMin);


		public static void RemoveOneSpace(this ref Rect self, GeneralDirection2D side = GeneralDirection2D.Up) =>
			self.RemoveSpace(EditorGUIUtility.standardVerticalSpacing, side);

		public static void RemoveSpace(this ref Rect self, float space, GeneralDirection2D side = GeneralDirection2D.Up)
		{
			if (side is GeneralDirection2D.Up)
			{
				self.y += space;
				self.height -= space;
			}
			else if (side is GeneralDirection2D.Down)
			{
				self.height -= space;
			}
			else if (side is GeneralDirection2D.Left)
			{
				self.x += space;
				self.width -= space;
			}
			else if (side is GeneralDirection2D.Right)
			{
				self.width -= space;
			}
		}

		public static Rect SliceOutLine(this ref Rect self, GeneralDirection2D side = GeneralDirection2D.Up, bool addSpace = true) =>
			self.SliceOut(EditorGUIUtility.singleLineHeight, side, addSpace);

		public static Rect SliceOut(this ref Rect self, float pixels, GeneralDirection2D side = GeneralDirection2D.Up, bool addSpace = true)
		{
			Rect slice = self;
			if (side is GeneralDirection2D.Up or GeneralDirection2D.Down)
			{
				slice.height = pixels;

				float newHeight = self.height - pixels;
				if (addSpace)
					newHeight -= EditorGUIUtility.standardVerticalSpacing;
				self.height = Mathf.Max(0, newHeight);

				if (side == GeneralDirection2D.Up)
				{
					self.y += pixels;

					if (addSpace)
						self.y += EditorGUIUtility.standardVerticalSpacing;
				}
				else
				{

					if (newHeight < 0)
						self.y -= newHeight;

					slice.y = self.yMax;
					if (addSpace)
						slice.y += EditorGUIUtility.standardVerticalSpacing;
				}
			}
			else
			{
				slice.x = pixels;
				float newWidth = self.width - pixels;
				if (addSpace)
					newWidth -= EditorGUIUtility.standardVerticalSpacing;
				self.width = Mathf.Max(0, newWidth);

				if (side == GeneralDirection2D.Right)
				{
					self.x += pixels;
					if (addSpace)
						self.x += EditorGUIUtility.standardVerticalSpacing;
				}
				else
				{
					if (newWidth < 0)
						self.x -= newWidth;

					slice.x = self.xMax;
					if (addSpace)
						slice.x += EditorGUIUtility.standardVerticalSpacing;

				}
			}

			return slice;
		}
	}
}

