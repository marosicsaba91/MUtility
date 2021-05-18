using System;
using UnityEngine;

namespace MUtility
{
public interface IInspectorFloatProperty : IInspectorProperty<float> { 
	bool TryGetRange(object parentObject, out float min, out float max); 
}

[Serializable]public abstract class InspectorFloat<TParentObject> : InspectorElement<TParentObject>, IInspectorFloatProperty
{
	[SerializeField] float value;
	public event Action<float> ValueChanged;
	public float GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, float value) => SetValue((TParentObject)  parentObject, value);
	public bool TryGetRange(object parentObject, out float min, out float max) => 
		TryGetRange((TParentObject) parentObject,  out min, out max);
 	
	public float Value
	{
		get => GetValue(ParentObject);
		set => SetValue(ParentObject, value);
	}
	
	protected virtual float GetValue(TParentObject parentObject) => value;

	protected virtual void SetValue(TParentObject parentObject, float value)
	{
		if (this.value == value) return;
		this.value = value;
		InvokeValueChanged();
	}
	
	protected void InvokeValueChanged() => ValueChanged?.Invoke(value);

	protected virtual bool TryGetRange(TParentObject container, out float min, out float max)
	{
		min = 0;
		max = 0;
		return false;
	}

	public static implicit operator float(InspectorFloat<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}
}