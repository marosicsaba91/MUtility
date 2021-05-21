using System;
using UnityEngine;

namespace MUtility
{
public interface IInspectorProperty<T>
{
    T GetValue(object parentObject);
    void SetValue(object parentObject, T value);
}


[Serializable]
public class InspectorProperty<TParentObject, TPropertyType> : 
    InspectorElement<TParentObject>,  IInspectorProperty<TPropertyType>
{
    [SerializeField] protected TPropertyType value;

    public delegate void ValueChangedDelegate(TParentObject parent, TPropertyType oldValue, TPropertyType newValue);
    
    public ValueChangedDelegate valueChanged;
    public TPropertyType GetValue(object parentObject) => GetValue((TParentObject) parentObject);
    public void SetValue(object parentObject, TPropertyType value)
    {
        if (Equals(this.value, value)) return;
        TPropertyType oldValue = this.value;
        SetValue((TParentObject) parentObject, value);
        valueChanged?.Invoke((TParentObject) parentObject, oldValue, value);
    }
 
    public TPropertyType Value
    {
        get => GetValue(ParentObject);
        set => SetValue(ParentObject, value);
    }
	    
    protected virtual TPropertyType GetValue(TParentObject parentObject) => value;

    protected virtual void SetValue(TParentObject parentObject, TPropertyType value) => this.value = value;
 

    public static implicit operator TPropertyType(InspectorProperty<TParentObject, TPropertyType> obj) => 
        obj.GetValue(obj.ParentObject);
}
 

}
    

