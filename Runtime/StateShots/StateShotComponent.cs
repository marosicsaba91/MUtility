using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{

	public class StateShotComponent : MonoBehaviour
	{
		public StateShot stateFile;
		bool _recordAtFixedUpdate = false;

		[EnableIf(nameof(HasStateFile))]
		[SerializeField] DisplayMember recordState = new DisplayMember(nameof(RecordStateAsync));
		[EnableIf(nameof(HasStateFile))]
		[SerializeField] DisplayMember apply = new DisplayMember(nameof(ApplyState));

		public bool HasStateFile => stateFile != null;

		public void RecordStateAsync()
		{
			if (Application.isPlaying)
				_recordAtFixedUpdate = true;
			else
				RecordState();
		}

		public void RecordState()
		{
			stateFile.Record(gameObject);
			_recordAtFixedUpdate = false;
			SetDirty(stateFile);
		}

		protected static void SetDirty(ScriptableObject record)
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(record);
			AssetDatabase.Refresh();
#endif
		}

		void FixedUpdate()
		{
			if (_recordAtFixedUpdate)
				RecordState();
		}

		void ApplyState() => stateFile.ApplyToObject(gameObject);
	}
}
