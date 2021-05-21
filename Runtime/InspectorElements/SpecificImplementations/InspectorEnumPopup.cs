using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class InspectorEnumPopup<TParentObject, TEnumType> : InspectorPopup<TParentObject> 
    where TEnumType : Enum
{
    [SerializeField] TEnumType value;
    public event Action<TEnumType> ValueChanged;
    protected sealed override IEnumerable<object> Elements(TParentObject parentObject) => 
        Enum.GetValues(typeof(TEnumType)).Cast<object>();
    
    public sealed override int GetSelectedElement(TParentObject parentObject) => 
        Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>().ToArray().IndexOf(GetValue(parentObject));

    public sealed override void SetSelectedElement(TParentObject parentObject, int index)
    {
        TEnumType[] elements = Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>().ToArray();
        if(elements.Length == 0) return;
        if (index < 0) index = 0;
        if (index > elements.Length) index = elements.Length - 1;
        SetValue(parentObject, Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>().ToArray()[index]);
    }
 	
    public TEnumType Value
    {
        get => GetValue((TParentObject)ParentObject);
        set => SetValue((TParentObject)ParentObject, value);
    }
    
    protected virtual TEnumType GetValue(TParentObject parentObject) => value;

    protected virtual void SetValue(TParentObject parentObject, TEnumType value)
    {
        if (this.value.Equals(value)) return;
        this.value = value;
        InvokeValueChanged();
    }
    protected void InvokeValueChanged() => ValueChanged.EditorInvoke(value);


    public static implicit operator TEnumType(InspectorEnumPopup<TParentObject, TEnumType> obj) =>
        obj.GetValue((TParentObject) obj.ParentObject);
}
}