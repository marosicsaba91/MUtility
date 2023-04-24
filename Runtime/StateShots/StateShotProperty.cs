using System;
using UnityEngine;

namespace MUtility
{

	public abstract class StateShotProperty
	{
		internal abstract void RecordObject(GameObject gameObject);
		internal abstract void ApplyState(GameObject gameObject);
		internal abstract bool CanRecord { get; }
		internal abstract Type ShotType { get; }
		internal abstract void SetStateShot(object shot);
	}

	[Serializable]
	public abstract class StateShotProperty<TStateShot> : StateShotProperty where TStateShot : StateShot
	{
		public TStateShot stateShot = default;
		internal sealed override bool CanRecord => stateShot != null;
		internal sealed override void RecordObject(GameObject gameObject) => stateShot.Record(gameObject);
		internal sealed override void ApplyState(GameObject gameObject) => stateShot.ApplyToObject(gameObject);
		internal sealed override void SetStateShot(object shot) => stateShot = (TStateShot)shot;
		internal sealed override Type ShotType => typeof(TStateShot);
	}
}