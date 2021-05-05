using System;
using System.Collections.Generic;
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