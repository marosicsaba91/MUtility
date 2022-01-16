using System;
using Object = UnityEngine.Object;

namespace MUtility
{
[Serializable] public abstract class InspectorString<TParentObject> : InspectorProperty<TParentObject, string>
{ 
}
[Serializable] public class InspectorString : InspectorString<Object> { }
}