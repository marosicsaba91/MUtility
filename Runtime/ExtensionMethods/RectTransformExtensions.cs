using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{

	public static class RectTransformExtensions
	{
		public static void SetLeft(this RectTransform rt, float left)
		{
			rt.offsetMin = new Vector2(left, rt.offsetMin.y);
		}

		public static void SetRight(this RectTransform rt, float right)
		{
			rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
		}

		public static void SetTop(this RectTransform rt, float top)
		{
			rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
		}

		public static void SetBottom(this RectTransform rt, float bottom)
		{
			rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
		}

		public static void Split(this Rect inputRect, float rate, out Rect first, out Rect second) =>
			Split(inputRect, rate, Space, out first, out second);

		public static float Space
		{
			get
			{
#if UNITY_EDITOR
				return EditorGUIUtility.standardVerticalSpacing;
#else
            return 2;
#endif
			}
		}


		public static void Split(this Rect inputRect, float rate, float space, out Rect first, out Rect second)
		{
			rate = Mathf.Clamp01(rate);
			first = inputRect;
			first.width = (inputRect.width - space) * rate;
			second = inputRect;
			second.x = first.xMax + space;
			second.width = (inputRect.width - space) * (1 - rate);
		}

		public static List<Rect> Split(this Rect inputRect, params (int pixel, int weight)[] lengths) =>
			Split(inputRect, Space, lengths);
		public static List<Rect> Split(this Rect inputRect, float space, params (int pixel, int weight)[] lengths)
		{
			if (lengths.Length == 0)
				return new List<Rect>();
			if (lengths.Length == 1)
				return new List<Rect> { inputRect };
			float fullWidth = inputRect.width - (lengths.Length - 1) * space;

			float fullWidthControlledByWeight = fullWidth;
			float allWeight = 0;
			foreach ((int pixel, int weight) w in lengths)
			{
				fullWidthControlledByWeight -= w.pixel;
				allWeight += w.weight;
			}

			float unityOtWeightInPixels =
				allWeight == 0 ? 0 : fullWidthControlledByWeight / allWeight;

			float x = inputRect.x;
			float y = inputRect.y;
			float h = inputRect.height;
			var result = new List<Rect>();
			foreach ((int pixel, int weight) w in lengths)
			{
				float pixels = w.pixel + w.weight * unityOtWeightInPixels;
				result.Add(new Rect(x, y, pixels, h));
				x += pixels + space;
			}

			return result;
		}
	}
}