using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class InspectorVector2<TParentObject> : InspectorElement<TParentObject>, IInspectorProperty<Vector2>
{
	[SerializeField]  Vector2 value;
	public event Action<Vector2> ValueChanged;
	
	public Vector2 GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, Vector2 value) => SetValue((TParentObject)  parentObject, value);
	
	public Vector2 Value
	{
		get => GetValue(ParentObject);
		set => SetValue(ParentObject, value);
	}
	
	protected virtual Vector2 GetValue(TParentObject parentObject) => value;

	protected virtual void SetValue(TParentObject parentObject, Vector2 value)
	{
		if(this.value == value) return;
		this.value = value;
		InvokeValueChanged();
	}
	protected void InvokeValueChanged() => ValueChanged.EditorInvoke(value);

	public static implicit operator Vector2(InspectorVector2<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}
}