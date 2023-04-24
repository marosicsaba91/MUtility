using System;
using UnityEngine;

namespace MUtility
{
	public static class StringExtensions
	{
		public static bool IsNullOrEmpty(this string s)
			=> string.IsNullOrEmpty(s);

		public static bool NotNullOrEmpty(this string s)
			=> !s.IsNullOrEmpty();

		public static string Colored(this string message, Color color) => $"<color={color.ToHex()}>{message}</color>";
		public static string Bold(this string str) => $"<b>{str}</b>";
		public static string Color(this string str, string clr) => $"<color={clr}>{str}</color>";
		public static string Italic(this string str) => $"<i>{str}</i>";
		public static string Size(this string str, int size) => $"<size={size}>{str}</size>";

		public static int Width(this string str, Font font = null)
		{
			if (font == null)
			{
				try
				{
					font = GUI.skin.font;
				}
				catch (ArgumentException) { }
			}

			if (font == null)
				return -1;

			int totalLength = 0;

			foreach (char c in str)
			{
				font.GetCharacterInfo(c, out CharacterInfo characterInfo, font.fontSize);
				totalLength += characterInfo.advance;
			}

			return totalLength;
		}
	}
}