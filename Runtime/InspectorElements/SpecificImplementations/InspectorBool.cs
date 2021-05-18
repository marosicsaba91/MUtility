using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class InspectorBool<TParentObject> : InspectorElement<TParentObject>, IInspectorProperty<bool>
{
	[SerializeField] bool value;
	public event Action<bool> ValueChanged;
	public bool GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, bool value) => SetValue((TParentObject)  parentObject, value); 
  	
	public bool Value
	{
		get => GetValue(ParentObject);
		set => SetValue(ParentObject, value);
	}
	
	protected virtual bool GetValue(TParentObject parentObject) => value;

	protected virtual void SetValue(TParentObject parentObject, bool value)
	{
		if (this.value == value) return;
		this.value = value;
		InvokeValueChanged();
	}
	protected void InvokeValueChanged() => ValueChanged.EditorInvoke(value);
	
	public static implicit operator bool(InspectorBool<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}
}