using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	public static class ScriptableObjectUtility
	{
		static readonly Dictionary<Type, object> resources = new();

		public static TScriptable GetFromResources<TScriptable>() where TScriptable : ScriptableObject
		{
			if (resources.TryGetValue(typeof(TScriptable), out object array))
				return ((TScriptable[])array).FirstOrDefault();

			TScriptable[] allResources = Resources.LoadAll<TScriptable>("");
			resources.Add(typeof(TScriptable), allResources);

			return allResources.FirstOrDefault();
		}


		public static TScriptable[] GetAllFromResources<TScriptable>() where TScriptable : ScriptableObject
		{
			if (resources.TryGetValue(typeof(TScriptable), out object array))
				return ((TScriptable[])array);

			TScriptable[] allResources = Resources.LoadAll<TScriptable>("");
			resources.Add(typeof(TScriptable), allResources);

			return allResources;
		}

		public static T[] FindAllInAssets_EditorOnly<T>() where T : ScriptableObject
		{
#if UNITY_EDITOR
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}", null);
			T[] result = new T[guids.Length];
			for (int i = 0; i < guids.Length; i++)
			{
				string guid1 = guids[i];
				string path = AssetDatabase.GUIDToAssetPath(guid1);
				result[i] = AssetDatabase.LoadAssetAtPath<T>(path);
			}
			return result;
#else
			Debug.LogError("ScriptableObjectUtility.FindAllInAssets_EditorOnly is only available in the editor.");
			return Array.Empty<T>();
#endif
		}
	}
}