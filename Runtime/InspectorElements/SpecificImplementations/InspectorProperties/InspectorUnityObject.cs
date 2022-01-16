using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace MUtility
{
public interface IInspectorUnityObject :  IInspectorProperty<Object>
{  
    Type ContentType { get; }
}

[Serializable] public abstract class InspectorUnityObject<TParentObject, TUnityObject> : 
    InspectorProperty<TParentObject, TUnityObject>, IInspectorUnityObject where TUnityObject: Object
{
    public new Object GetValue(object parentObject) => base.GetValue(parentObject);

    public void SetValue(object parentObject, Object newValue) => base.SetValue(parentObject, (TUnityObject) newValue);
    public new IList<Object> PopupElements(object parentObject) => base.PopupElements(parentObject)?.Cast<Object>().ToList();

    public virtual Type ContentType => typeof(TUnityObject); 
}

[Serializable] public abstract class InspectorUnityObject<TParentObject> :  InspectorUnityObject<TParentObject, Object> 
{     
    public Type type = typeof(Object);
    public override Type ContentType => type;
}

[Serializable] public class InspectorUnityObject :  InspectorUnityObject<object> 
{ }
}