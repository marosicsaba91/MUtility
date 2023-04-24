using System;
using UnityEngine;

namespace MUtility
{
	public enum DisplayColor
	{
		White,
		Black,
		Gray,
		Red,
		Green,
		Blue,
		Cyan,
		Magenta,
		Yellow,
		Brown,
		Orange,
		Purple,
	}
	public class ColorAttribute : FormattingAttribute
	{
		static Color ToColor(DisplayColor displayColor)
		{
			switch (displayColor)
			{
				case DisplayColor.Black:
					return Color.black;
				case DisplayColor.White:
					return Color.white;
				case DisplayColor.Gray:
					return Color.gray;
				case DisplayColor.Red:
					return new Color(1f, 0.42f, 0.35f);
				case DisplayColor.Green:
					return new Color(0.54f, 0.75f, 0.35f);
				case DisplayColor.Blue:
					return new Color(0.27f, 0.51f, 0.82f);
				case DisplayColor.Cyan:
					return new Color(0.4f, 0.93f, 0.92f);
				case DisplayColor.Magenta:
					return new Color(1f, 0.49f, 0.64f);
				case DisplayColor.Yellow:
					return new Color(1f, 0.73f, 0.25f);
				case DisplayColor.Brown:
					return new Color(0.48f, 0.36f, 0.29f);
				case DisplayColor.Orange:
					return new Color(0.96f, 0.6f, 0.36f);
				case DisplayColor.Purple:
					return new Color(0.51f, 0.43f, 0.78f);
				default:
					return Color.white;
			}
		}


		readonly DisplayColor? _fixColor;
		readonly string _colorMember;
		Func<object, Color> _colorGetter;
		bool _initialized;

		public ColorAttribute(string colorMemberName)
		{
			_colorMember = colorMemberName;
			_initialized = false;
		}

		public ColorAttribute(DisplayColor fixColor)
		{
			_fixColor = fixColor;
			_initialized = true;
		}

		public void Initialize(object owner)
		{
#if UNITY_EDITOR
			if (_initialized)
				return;

			InspectorDrawingUtility.TryGetAGetterFromMember(owner.GetType(), _colorMember, out _colorGetter);
			_initialized = true;
#endif
		}

		public bool TryGetColor(object owner, out Color color)
		{
			if (_fixColor.HasValue)
			{
				color = ToColor(_fixColor.Value);
				return true;
			}

			if (_colorGetter != null)
			{
				color = _colorGetter(owner);
				return true;
			}

			color = default;
			return false;
		}
	}
}