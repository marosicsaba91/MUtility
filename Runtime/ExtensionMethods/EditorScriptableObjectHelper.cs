#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MUtility
{
	public static class EditorScriptableObjectHelper
	{
		public static TScriptableObject Find<TScriptableObject>() where TScriptableObject : ScriptableObject
		{
			string settingGuid = AssetDatabase.FindAssets($"t:{nameof(TScriptableObject)}").First();
			string settingPath = AssetDatabase.GUIDToAssetPath(settingGuid);
			return AssetDatabase.LoadAssetAtPath<TScriptableObject>(settingPath);
		}

		public static bool TryFind<TScriptableObject>(out TScriptableObject result) where TScriptableObject : ScriptableObject
		{
			result = null;
			string[] settingGuids = AssetDatabase.FindAssets($"t:{nameof(TScriptableObject)}");
			if (settingGuids.Length == 0)
				return false;
			string settingGuid = settingGuids.First();
			string settingPath = AssetDatabase.GUIDToAssetPath(settingGuid);
			if (string.IsNullOrEmpty(settingPath))
				return false;
			result = AssetDatabase.LoadAssetAtPath<TScriptableObject>(settingPath);

			return result != null;
		}

		public static IEnumerable<TScriptableObject> FindAll<TScriptableObject>() where TScriptableObject : ScriptableObject
		{
			string[] settingGuids = AssetDatabase.FindAssets($"t:{nameof(TScriptableObject)}");
			if (settingGuids.Length == 0)
				yield break;
			foreach (string settingGuid in settingGuids)
			{
				string settingPath = AssetDatabase.GUIDToAssetPath(settingGuid);
				if (string.IsNullOrEmpty(settingPath))
					continue;
				yield return AssetDatabase.LoadAssetAtPath<TScriptableObject>(settingPath);
			}
		}
	}
}
#endif