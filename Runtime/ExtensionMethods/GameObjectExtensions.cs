using UnityEngine;

namespace MUtility
{
public static class GameObjectExtensions
{
    public static void FindOrCreateComponentInSelf<T>(this GameObject gameObject, ref T component) where T : Component
    {
        if (component == null)
            component = gameObject.GetComponent<T>();
        if (component == null)
            component = gameObject.AddComponent<T>();
    }

    public static void FindOrCreateReferenceInChildren<T>(
        this GameObject gameObject,
        ref T component,
        string newObjectName = null)
        where T : Component
    {
        if (component == null)
            component = gameObject.GetComponentInChildren<T>();
        
        if (component != null) return;

        string name = string.IsNullOrEmpty(newObjectName) ? typeof(T).Name : newObjectName;
        var child = new GameObject(name);
        child.transform.SetParent(gameObject.transform);
        child.transform.localPosition = UnityEngine.Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = UnityEngine.Vector3.one;
        component = child.AddComponent<T>();
    }
}
}