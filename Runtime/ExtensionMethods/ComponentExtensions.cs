using System;
using UnityEngine;

namespace MUtility
{
	public static class ComponentExtensions
	{
		// Returns a unique string ID for the given MonoBehaviour
		public static string GetUniqueID(this Component monoBehaviour)
		{
			// Get the type, name, and GUID of the GameObject
			Type type = monoBehaviour.GetType();
			GameObject gameObject = monoBehaviour.gameObject;
			string name = gameObject.name;
			int guid = gameObject.GetInstanceID();

			// Combine the type, name, and GUID into a single string and return it
			string id = $"{type.FullName}_{name}_{guid}";
			return id;
		}
	}
}