#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
	[CustomPropertyDrawer(typeof(DisplayMember))]
	public class DisplayMemberDrawer : PropertyDrawer
	{
		string _memberName;

		Type _type;
		Type _ownerType;
		DisplayMember _displayMember;
		MethodInfo _buttonMethodInfo;
		Func<float, float> _floatFunction;
		CurveEditorPreview _curvePreview = new CurveEditorPreview();
		FieldInfo _fieldInfo;
		PropertyInfo _propertyInfo;
		Object _serializedObject;
		object _owner;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (_serializedObject == null)
				return;

			label = _displayMember.useMemberNameAsLabel
				? new GUIContent(ObjectNames.NicifyVariableName(_displayMember.memberName))
				: label;

			if (_buttonMethodInfo != null)
			{
				if (GUI.Button(position, label))
					_buttonMethodInfo.Invoke(_owner, Array.Empty<object>());

				return;
			}

			bool isExpanded = property.isExpanded;
			Undo.RecordObject(_serializedObject, "Inspector Member Changed");
			if (_floatFunction != null)
			{
				Rect content = EditorHelper.ContentRect(position);
				Rect labelRect = EditorHelper.LabelRect(position);
				property.isExpanded = EditorGUI.Foldout(labelRect, isExpanded, label);

				Vector2 size = new Vector2(1, 1);
				Rect area = new Rect(-(size / 2f), size);
				_curvePreview.zoom = _displayMember.functionZoom;
				_curvePreview.offset = _displayMember.functionOffset;
				_curvePreview.Draw(content, _floatFunction, area, EditorHelper.functionColor, isExpanded);
				property.isExpanded = isExpanded;
				_displayMember.functionOffset = _curvePreview.offset;
				_displayMember.functionZoom = _curvePreview.zoom;

				return;
			}

			if (_fieldInfo != null)
			{
				object oldValue = _fieldInfo.GetValue(_owner);
				object newValue = AnythingField(position, _type, oldValue, label, ref isExpanded);
				property.isExpanded = isExpanded;
				if (!Equals(oldValue, newValue))
					_fieldInfo.SetValue(_owner, newValue);
				return;
			}

			if (_propertyInfo != null)
			{
				object oldValue = _propertyInfo.GetValue(_owner);
				bool savedEnabled = GUI.enabled;
				if (_propertyInfo.SetMethod == null)
					GUI.enabled = false;

				object newValue = AnythingField(position, _type, oldValue, label, ref isExpanded);
				GUI.enabled = savedEnabled;
				if (!Equals(oldValue, newValue))
				{
					try
					{
						_propertyInfo.SetValue(_owner, newValue);
					}
					catch (Exception)
					{
						property.SetValue(newValue);
					}
				}
				property.isExpanded = isExpanded;
				return;
			}

			property.isExpanded = isExpanded;
			Error(position, label, $"No valid member named: {_displayMember.memberName}");
		}

		object AnythingField(Rect position, Type t, object value, GUIContent label, ref bool isExpanded)
		{
			try
			{
				if (t == typeof(bool))
					return EditorGUI.Toggle(position, label, (bool)value);
				if (t == typeof(int))
					return EditorGUI.IntField(position, label, (int)value);
				if (t == typeof(float))
					return EditorGUI.FloatField(position, label, (float)value);
				if (t == typeof(string))
					return EditorGUI.TextField(position, label, (string)value);
				if (t == typeof(Vector2))
					return EditorGUI.Vector2Field(position, label, (Vector2)value);
				if (t == typeof(Vector3))
					return EditorGUI.Vector3Field(position, label, (Vector3)value);
				if (t == typeof(Vector4))
					return EditorGUI.Vector4Field(position, label, (Vector4)value);
				if (t == typeof(Vector2Int))
					return EditorGUI.Vector2IntField(position, label, (Vector2Int)value);
				if (t == typeof(Vector3Int))
					return EditorGUI.Vector3IntField(position, label, (Vector3Int)value);
				if (t == typeof(Color))
					return EditorGUI.ColorField(position, label, (Color)value);
				if (t == typeof(Gradient))
					return EditorGUI.GradientField(position, label, (Gradient)value);
				if (t == typeof(Rect))
					return EditorGUI.RectField(position, label, (Rect)value);
				if (t == typeof(Bounds))
					return EditorGUI.BoundsField(position, label, (Bounds)value);
				if (t == typeof(AnimationCurve))
					return EditorGUI.CurveField(position, label, (AnimationCurve)value);
				if (t == typeof(double))
					return EditorGUI.DoubleField(position, label, (double)value);
				if (t == typeof(long))
					return EditorGUI.LongField(position, label, (long)value);
				if (t.IsSubclassOf(typeof(Object)))
					return EditorGUI.ObjectField(position, label, (Object)value, t, true);
				if (t == typeof(RectInt))
					return EditorGUI.RectIntField(position, label, (RectInt)value);
				if (t == typeof(BoundsInt))
					return EditorGUI.BoundsIntField(position, label, (BoundsInt)value);
				if (t.IsSubclassOf(typeof(Enum)))
					return EditorGUI.EnumPopup(position, label, (Enum)value);
				if (t == typeof(Matrix4x4))
					return Nice4X4MatrixDrawer.Draw(position, label, (Matrix4x4)value, ref isExpanded);  // Add Universal solution
			}
			catch (InvalidCastException)
			{
				_memberName = null;
			}

			Error(position, label, $" Type: {t} is not supported type for DisplayMember!");

			return null;
		}

		void Error(Rect position, GUIContent label, string message)
		{
			Rect labelPos = position;
			labelPos.width = EditorHelper.LabelWidth;
			Rect contentPos = EditorHelper.ContentRect(position);
			EditorGUI.LabelField(labelPos, label);
			EditorHelper.DrawErrorBox(contentPos);
			EditorGUI.LabelField(contentPos, message);
			_memberName = null;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{

			_displayMember = (DisplayMember)property.GetObjectOfProperty();
			if (_memberName != _displayMember.memberName)
			{
				_owner = property.GetObjectWithProperty();
				_ownerType = _owner.GetType();
				_memberName = _displayMember.memberName;
				if (TryGetMethodInfo(_ownerType, _memberName, out _buttonMethodInfo))
					_type = _buttonMethodInfo.ReturnType;
				else if (TryGetFunction(_owner, _ownerType, _memberName, out _floatFunction, out _curvePreview))
					_type = typeof(float);
				else if (TryGetFieldInfo(_ownerType, _memberName, out _fieldInfo))
					_type = _fieldInfo.FieldType;
				else if (TryGetPropertyInfo(_ownerType, _memberName, out _propertyInfo))
					_type = _propertyInfo.PropertyType;

				_serializedObject = property.serializedObject.targetObject;
			}

			if (_buttonMethodInfo != null)
				return EditorGUIUtility.singleLineHeight;

			if (_floatFunction != null)
				return property.isExpanded ? 120 : EditorGUIUtility.singleLineHeight;

			return AnythingHeight(_type, property);
		}

		float AnythingHeight(Type t, SerializedProperty property)
		{
			if (t == typeof(Rect) ||
				t == typeof(RectInt))
				return 2 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			if (t == typeof(Bounds) ||
				t == typeof(BoundsInt))
				return 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
			if (t == typeof(Matrix4x4))
				return Nice4X4MatrixDrawer.PropertyHeight(property);  // Add Universal solution

			return EditorGUIUtility.singleLineHeight;
		}

		static BindingFlags Binding => InspectorDrawingUtility.bindings;

		public static bool TryGetMethodInfo(Type ownerType, string name, out MethodInfo methodInfo)
		{
			MethodInfo method = ownerType.GetMethod(name, Binding);

			if (method != null && method.GetParameters().IsNullOrEmpty())
			{
				methodInfo = method;
				return true;
			}

			methodInfo = null;
			return false;
		}


		public static bool TryGetFunction(object owner, Type ownerType, string name,
			out Func<float, float> function, out CurveEditorPreview curve)
		{
			MethodInfo func = ownerType
				.GetMethods(Binding)
				.FirstOrDefault(m =>
					m.Name == name &&
					(m.ReturnType == typeof(float) || m.ReturnType == typeof(int)) &&
					m.GetParameters().Length == 1 &&
					m.GetParameters()[0].ParameterType == typeof(float));

			if (func != null)
			{
				float Function(float f)
				{
					object result = func.Invoke(owner, new object[] { f });
					Type resultType = result.GetType();
					if (resultType == typeof(int))
						return (int)result;
					return (float)result;
				}

				function = Function;
				curve = new CurveEditorPreview();
				return true;
			}

			function = null;
			curve = null;
			return false;
		}

		public static bool TryGetFieldInfo(Type ownerType, string name, out FieldInfo fieldInfo)
		{
			fieldInfo = ownerType.GetField(name, Binding);
			return fieldInfo != null;
		}

		public static bool TryGetPropertyInfo(Type ownerType, string name, out PropertyInfo propertyInfo)
		{
			PropertyInfo property = ownerType.GetProperty(name, Binding);

			if (property != null && property.GetMethod != null)
			{
				propertyInfo = property;
				return true;
			}

			propertyInfo = null;
			return false;
		}
	}
}
#endif