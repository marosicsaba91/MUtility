#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MUtility.Editor
{
	[CustomPropertyDrawer(typeof(RangeAttribute))] // Target the attribute, not just the type
	internal class UpdatedDrawerForRange : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			RangeAttribute range = (RangeAttribute)attribute;

			if (property.propertyType == SerializedPropertyType.Float)
				EditorGUI.Slider(position, property, range.min, range.max, label);

			else if (property.propertyType == SerializedPropertyType.Integer)

				EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);

			else if(property.type is "RangedFloat" or "RangedInt")
				RangedNumberDrawer.DrawRanged(position, property, label);
			
			else			
				EditorGUI.LabelField(position, label.text, "Use Range with float, int RangedFloat or RangedInt.");
		}
	}
}
#endif