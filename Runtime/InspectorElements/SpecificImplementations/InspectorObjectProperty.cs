using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace MUtility
{
public interface IInspectorUnityObject
{
    public abstract IList<Object> NotNullPopupElements(object container);
    public abstract Object GetValue(object container);
    public abstract void SetValue(object container, Object value);
    public abstract Type ContentType { get; }
}

public abstract class InspectorUnityObject<TParentObject> : InspectorElement<TParentObject>, IInspectorUnityObject 
{
    public Object GetValue(object parentObject) => GetValue((TParentObject) parentObject) ;

    public void SetValue(object parentObject, Object value) => SetValue((TParentObject) parentObject,  value);

    public abstract  Type ContentType { get; }
    public IList<Object> NotNullPopupElements(object parentObject) => NotNullPopupElements((TParentObject) parentObject); 
    
    protected abstract Object GetValue(TParentObject parentObject);
    protected virtual void SetValue(TParentObject parentObject, Object value) { }
    protected virtual IList<Object> NotNullPopupElements(TParentObject parentObject) => null;  
}
}