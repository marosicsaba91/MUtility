using System; 

namespace MUtility
{
[Serializable] public abstract class BoolProperty<TParentObject> : InspectorProperty<TParentObject, bool>
{ }

[Serializable] public class BoolProperty : BoolProperty<object> { }
}