using System;
using UnityEngine;

namespace Utility.SimpleComponents
{
	[ExecuteInEditMode]
	public class TransformEditorLocker : MonoBehaviour
	{
#if UNITY_EDITOR
		enum LockSate
		{
			Lock,
			GridLock,
			Unlock
		}

		[Serializable]
		struct LockSettings
		{
			public LockSate x;
			public LockSate y;
			public LockSate z;
			public Vector3 gridSize;

			public LockSettings(LockSate x, LockSate y, LockSate z, Vector3 gridSize)
			{
				this.x = x;
				this.y = y;
				this.z = z;
				this.gridSize = gridSize;
			}

			public Vector3 Lock(Vector3 input, Vector3 original)
			{
				if (!AnyLock)
					return input;

				Vector3 vector;
				vector.x = Lock(x, input.x, original.x, gridSize.x);
				vector.y = Lock(y, input.y, original.y, gridSize.y);
				vector.z = Lock(z, input.z, original.z, gridSize.z);

				return vector;
			}

			static float Lock(LockSate state, float input, float original, float grid) =>
				state == LockSate.Lock ? original :
				state == LockSate.GridLock ? input - input % grid :
				input;

			public bool AnyLock => x != LockSate.Unlock || y != LockSate.Unlock || z != LockSate.Unlock;
		}

		[SerializeField]
		LockSettings position = new LockSettings(LockSate.Lock, LockSate.Lock, LockSate.Lock, Vector3.one);

		[SerializeField]
		LockSettings rotation =
			new LockSettings(LockSate.Lock, LockSate.Lock, LockSate.Lock, new Vector3(45, 45, 45));

		[SerializeField]
		LockSettings scale =
			new LockSettings(LockSate.Lock, LockSate.Lock, LockSate.Lock, new Vector3(0.1f, 0.1f, 0.1f));

		Vector3 _originalPosition;
		Quaternion _originalRotation;
		Vector3 _originalScale;

		void OnEnable() => OnValidate();

		void OnValidate()
		{
			Transform t = transform;
			_originalPosition = t.localPosition;
			_originalRotation = t.localRotation;
			_originalScale = t.localScale;
		}

		void Update()
		{
			if (Application.isPlaying)
				return;

			Transform t = transform;

			t.localPosition = position.Lock(t.localPosition, _originalPosition);

			Vector3 originalRot = _originalRotation.eulerAngles;
			Vector3 rot = t.localRotation.eulerAngles;
			t.localRotation = Quaternion.Euler(rotation.Lock(rot, originalRot));

			t.localScale = scale.Lock(t.localScale, _originalScale);
		}
#endif
	}
}