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

[Serializable] public abstract class UnityObjectProperty<TParentObject, TUnityObject> : 
    InspectorProperty<TParentObject, TUnityObject>, IInspectorUnityObject where TUnityObject: Object
{
    public new Object GetValue(object container) => base.GetValue(container);

    public void SetValue(object container, Object value) => base.SetValue(container, (TUnityObject) value);

    public virtual Type ContentType => typeof(TUnityObject);
    public IList<Object> NotNullPopupElements(object parentObject) => NotNullPopupElements((TParentObject) parentObject);
    protected virtual IList<Object> NotNullPopupElements(TParentObject parentObject) => null; 
}

[Serializable] public abstract class UnityObjectProperty<TParentObject> :  UnityObjectProperty<TParentObject, Object> 
{     
    public Type type = typeof(Object);
    public override Type ContentType => type;
}

[Serializable] public class UnityObjectProperty :  UnityObjectProperty<object> 
{ }
}