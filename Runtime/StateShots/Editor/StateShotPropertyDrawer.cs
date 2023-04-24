#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MUtility.Editor
{
	[CustomPropertyDrawer(typeof(StateShotProperty), useForChildren: true)]
	public class StateShotPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;
			property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
			if (!property.isExpanded)
				return;
			EditorGUI.indentLevel++;
			var ssp = (StateShotProperty)property.GetObjectOfProperty();
			GameObject gameObject = ((Component)property.serializedObject.targetObject).gameObject;

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("stateShot"));

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			if (ssp.CanRecord)
			{
				if (GUI.Button(position, "Record GameObject to State Shot"))
					ssp.RecordObject(gameObject);
			}
			else
			{
				if (GUI.Button(position, "Create New Shot"))
					CreateNewShot(ssp, gameObject, ssp.ShotType);
			}

			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			if (GUI.Button(position, "Apply State Shot to GameObject"))
				ssp.ApplyState(gameObject);

			EditorGUI.indentLevel--;
		}


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			property.isExpanded
				? 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing
				: EditorGUIUtility.singleLineHeight;

		void CreateNewShot(StateShotProperty ssp, GameObject gameObject, Type t)
		{
			string openedPath = OpenedFolderPath();
			string path = EditorUtility.SaveFolderPanel(
				"Save new State Shot.",
				ToAssetPath(openedPath),
				"");
			if (path.Length == 0)
				return;

			var asset = (StateShot)ScriptableObject.CreateInstance(t);
			asset.Record(gameObject);
			ssp.SetStateShot(asset);

			path = ToAssetPath(path);
			path += "/New Shot.asset";
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();

			Selection.activeObject = asset;
			AssetDatabase.Refresh();
		}

		string OpenedFolderPath()
		{
			Type projectWindowUtilType = typeof(ProjectWindowUtil);
			MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod(
				"GetActiveFolderPath",
				BindingFlags.Static | BindingFlags.NonPublic);
			string openedPath = (string)getActiveFolderPath.Invoke(null, new object[0]);
			openedPath = ToAssetPath(openedPath);
			return openedPath;
		}

		string ToAssetPath(string path) => path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));
	}
}
#endif