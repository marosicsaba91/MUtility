using System;
using UnityEngine;

namespace MUtility
{
[Serializable] public abstract class InspectorVector2<TParentObject> : InspectorProperty<TParentObject, Vector2> { }

[Serializable] public class InspectorVector2 : InspectorVector2<object> { }
}