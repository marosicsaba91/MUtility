using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class Vector2Property<TParentObject> : InspectorProperty<TParentObject, Vector2> { }

[Serializable] public class Vector2Property : Vector2Property<object> { }
}