using System; 

namespace MUtility
{
[Serializable] public abstract class InspectorBool<TParentObject> : InspectorProperty<TParentObject, bool>
{ }

[Serializable] public class InspectorBool : InspectorBool<object> { }
}