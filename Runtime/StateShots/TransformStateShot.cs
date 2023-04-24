using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	[CreateAssetMenu(fileName = "Transform State Shot", menuName = "State Shots/Transform State Shot", order = 1)]
	public class TransformStateShot : StateShot
	{
		public List<SingleTransformState> transformStatesRecursively;

		protected override void RecordState(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			transformStatesRecursively = new List<SingleTransformState>();
			foreach (Transform child in transform.SelfAndAllChildrenRecursively())
				transformStatesRecursively.Add(new SingleTransformState(child));
		}

		protected override void ApplyStateToObject(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			int i = 0;
			foreach (Transform child in transform.SelfAndAllChildrenRecursively())
			{
				SingleTransformState saved = transformStatesRecursively[i];
				child.localPosition = saved.localPosition;
				child.localRotation = saved.localRotation;
				child.localScale = saved.localScale;
				i++;
			}
		}
	}

	[Serializable]
	public struct SingleTransformState
	{
		public Vector3 localPosition;
		public Quaternion localRotation;
		public Vector3 localScale;

		public SingleTransformState(Transform t)
		{
			localPosition = t.localPosition;
			localRotation = t.localRotation;
			localScale = t.localScale;
		}
	}
}