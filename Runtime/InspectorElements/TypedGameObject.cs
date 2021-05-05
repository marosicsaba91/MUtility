using System;
using UnityEngine;

namespace MUtility
{
[Serializable]
public abstract class TypedGameObject
{
    [SerializeField] protected GameObject gameObject;
    public GameObject GameObject => gameObject;

    public abstract bool TrySetGameObject(GameObject value);
}

[Serializable]
public abstract class TypedGameObject<TComponent> : TypedGameObject where TComponent:Component
{
    TComponent _component = null;

    TComponent Component
    {
        get
        {
            if (_component == null)
                _component = gameObject.GetComponent<TComponent>();
            return _component;
        }
    }

    public override bool TrySetGameObject(GameObject value)
    {
        if(value == gameObject) return false;
            
        if (value == null)
        {
            gameObject = null;
            return true;
        }

        var implementationProto = value.GetComponent<TComponent>();
        if (implementationProto == null)
            return false;

        gameObject = value;
        return true;
    }
}
}