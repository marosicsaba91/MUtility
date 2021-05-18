using System;
using UnityEngine;

namespace MUtility
{
public interface IInspectorInt : IInspectorProperty<int>
{
	bool TryGetRange(object parentObject,  out int min, out int max);
}

[Serializable]public abstract class InspectorInt<TParentObject> : InspectorElement<TParentObject>, IInspectorInt
{
	[SerializeField] int value;
	public event Action<int> ValueChanged;
	
	public int GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, int value) => SetValue((TParentObject)  parentObject, value);
	public bool TryGetRange(object parentObject, out int min, out int max) => 
		TryGetRange((TParentObject) parentObject, out min, out max);
	
	public int Value
	{
		get => GetValue(ParentObject);
		set => SetValue(ParentObject, value);
	}

	protected virtual int GetValue(TParentObject parentObject) => value;

	protected virtual void SetValue(TParentObject parentObject, int value)
	{
		if (this.value == value) return;
		this.value = value;
		InvokeValueChanged();
	}
	protected void InvokeValueChanged() => ValueChanged?.EditorInvoke(value);

	
	protected virtual bool TryGetRange(TParentObject container, out int min, out int max)
	{
		min = default;
		max = default;
		return false;
	}
	
	public static implicit operator int(InspectorInt<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}
}