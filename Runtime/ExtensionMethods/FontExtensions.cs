
using UnityEngine;

namespace MUtility
{
	public static class FontExtensions
	{
		public static int GetTextLength(this Font font, string message)
		{
			int totalLength = 0;
			foreach (char c in message)
			{
				font.GetCharacterInfo(c, out CharacterInfo characterInfo, font.fontSize);
				totalLength += characterInfo.advance;
			}
			return totalLength;
		}
	}
}