#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MUtility
{
	public class BoolColumn<TRow> : ValueColumn<TRow, bool>
	{
		public BoolColumn(
			Func<TRow, bool> valueGetter,
			ColumnInfo columnInfo = null) :
			base(valueGetter, columnInfo)
		{ }

		protected override void DrawCell(Rect position, bool value, GUIStyle style)
		{
			const float checkBoxWidth = 14;
			GUI.enabled = false;
			float x = position.x + (position.width - checkBoxWidth) / 2;
			Rect pos = new Rect(x, position.y, checkBoxWidth, position.height);
			EditorGUI.Toggle(pos, value, style);
			GUI.enabled = true;
		}


		protected override GUIStyle GetDefaultStyle() => new GUIStyle(GUI.skin.toggle);

		public override Alignment HeaderAlignment => Alignment.Center;
	}

}
#endif