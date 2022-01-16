using System; 

namespace MUtility
{
[Serializable] public abstract class InspectoreBool<TParentObject> : InspectorProperty<TParentObject, bool>
{ }

[Serializable] public class InspectoreBool : InspectoreBool<object> { }
}