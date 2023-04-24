using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MUtility
{
	public abstract class StateShot : ScriptableObject
	{
		public TimeStamp timeStamp;

		public void Record(GameObject gameObject)
		{
#if UNITY_EDITOR
			Undo.RecordObject(this, "World State Recorded");
#endif
			RecordState(gameObject);
			timeStamp = TimeStamp.Now();
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}

		public void ApplyToObject(GameObject gameObject)
		{
#if UNITY_EDITOR
			Undo.RecordObjects(gameObject.transform.GetAllUnityObjects(), "World State Shot Applied");
#endif
			ApplyStateToObject(gameObject);
		}

		protected abstract void RecordState(GameObject gameObject);
		protected abstract void ApplyStateToObject(GameObject gameObject);

	}
}