using UnityEngine; 
using System.Collections.Generic;
using System;
using System.Linq;

namespace MUtility
{
	public static class ScriptableObjectUtility
	{
		static readonly Dictionary<Type, object> resources = new Dictionary<Type, object>();

		public static TScriptable GetFromResources<TScriptable>() where TScriptable : ScriptableObject
		{
			if(resources.TryGetValue(typeof(TScriptable), out object array))
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
	}
}