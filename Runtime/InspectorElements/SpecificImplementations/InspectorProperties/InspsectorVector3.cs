using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class InspectorVector3<TParentObject> : InspectorProperty<TParentObject, UnityEngine.Vector3>
{
}
[Serializable] public class InspectorVector3 : InspectorVector3<object> { }
}