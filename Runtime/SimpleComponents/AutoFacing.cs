using MUtility;
using UnityEngine;

[ExecuteAlways]
public class AutoFacing : MonoBehaviour
{
	// TODO: Rigidbody Support

	public enum TargetType
	{
		FaceTarget,
		SameDirectionAsTarget,
		SameDirectionAsTargetWithOffset
	}

	[SerializeField] Transform target;
	[SerializeField] TargetType targetType;
	[SerializeField, ShowIf(nameof(HaveOffset))] Vector3 offset = new Vector3(0, 0, -10);

	[SerializeField] bool inEditor = false;

	bool HaveOffset => targetType == TargetType.SameDirectionAsTargetWithOffset;

	void LateUpdate()
	{
		if (Application.isEditor && !inEditor)
			return;

		if (target == null)
			return;

		if (targetType == TargetType.FaceTarget)
		{
			transform.LookAt(target, transform.up);
		}
		else
		{
			transform.rotation = Quaternion.LookRotation(target.forward, transform.up);
			;

			if (targetType == TargetType.SameDirectionAsTargetWithOffset)
			{
				transform.position = target.position + target.TransformVector(offset);
			}
		}
	}
}
