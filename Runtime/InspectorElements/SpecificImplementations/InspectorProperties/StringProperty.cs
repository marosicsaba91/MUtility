using System;
using Object = UnityEngine.Object;

namespace MUtility
{
[Serializable] public abstract class StringProperty<TParentObject> : InspectorProperty<TParentObject, string>
{ 
}
[Serializable] public class StringProperty : StringProperty<Object> { }
}