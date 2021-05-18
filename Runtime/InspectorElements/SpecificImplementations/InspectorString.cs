using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class InspectorString<TParentObject> : InspectorElement<TParentObject>,  IInspectorProperty<string>
{
	[SerializeField] string value;
	public event Action<string> ValueChanged;
	
	public string GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, string value) => SetValue((TParentObject)  parentObject, value);
	
	public string Value
	{
		get => GetValue(ParentObject);
		set => SetValue(ParentObject, value);
	}
	protected virtual string GetValue(TParentObject parentObject) => value;

	protected virtual void SetValue(TParentObject parentObject, string value)
	{
		if (this.value == value) return;
		this.value = value;
		InvokeValueChanged();
	}
	protected void InvokeValueChanged() => ValueChanged?.Invoke(value);

	public static implicit operator string(InspectorString<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}
}