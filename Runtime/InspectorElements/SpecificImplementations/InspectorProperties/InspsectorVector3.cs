using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class Vector3Property<TParentObject> : InspectorProperty<TParentObject, Vector3>
{
}
[Serializable] public class Vector3Property : Vector2Property<object> { }
}