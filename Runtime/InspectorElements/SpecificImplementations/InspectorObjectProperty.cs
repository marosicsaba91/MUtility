using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
public interface IInspectorUnityObject
{
    IList<Object> NotNullPopupElements(object container);
    Object GetValue(object container);
    void SetValue(object container, Object value);
    Type ContentType { get; }
}

[Serializable] public abstract class InspectorUnityObject<TParentObject> : InspectorElement<TParentObject>, IInspectorUnityObject 
{
    [SerializeField] Object value;
    public event Action<Object> ValueChanged;
    
    public Object GetValue(object parentObject) => GetValue((TParentObject) parentObject) ;

    public void SetValue(object parentObject, Object value) => SetValue((TParentObject) parentObject,  value);
    
     
    public abstract  Type ContentType { get; }
    public IList<Object> NotNullPopupElements(object parentObject) => NotNullPopupElements((TParentObject) parentObject);

    public Object Value
    {
        get => GetValue(ParentObject);
        set => SetValue(ParentObject, value);
    }
    protected virtual Object GetValue(TParentObject parentObject) => value;

    protected virtual void SetValue(TParentObject parentObject, Object value)
    {
        if (this.value == value) return;
        this.value = value;
        InvokeValueChanged();
    }
    protected void InvokeValueChanged() => ValueChanged?.Invoke(value);

    protected virtual IList<Object> NotNullPopupElements(TParentObject parentObject) => null;

    public static implicit operator Object(InspectorUnityObject<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}
}