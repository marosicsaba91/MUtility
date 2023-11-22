using System;
using UnityEngine;

[Serializable]
public struct Position3
{
	public enum SourceType
	{
		Vector,
		Transform,
	}

	public Transform transformSource;
	public Vector3 vectorSource;
	public SourceType sourceType;

	public Position3(Vector3 vector)
	{
		sourceType = sourceType = SourceType.Vector;
		vectorSource = vector;
		transformSource = null;
	}
	public Position3(Transform transform)
	{
		sourceType = sourceType = SourceType.Transform;
		transformSource = transform;
		vectorSource = Vector3.zero;
	}

	public Vector3 Position =>
		sourceType == SourceType.Vector
			? vectorSource
			: transformSource == null
				? Vector3.zero
				: transformSource.position;
}