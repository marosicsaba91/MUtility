using UnityEngine;

namespace MUtility
{
[RequireComponent(typeof(PolygonComponent))]
public class PolygonRenderer : MonoBehaviour
{
    [SerializeField] PolygonComponent polygon;

    void OnValidate()
    {
        if (polygon == null)
            polygon = GetComponent<PolygonComponent>();
    }
}
}