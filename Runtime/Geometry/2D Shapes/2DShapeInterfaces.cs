using UnityEngine;

namespace MUtility
{
    public interface I2DCircumference 
    {
        float Circumference { get; }
    }

    public interface I2DArea 
    {
        float Area { get; }
        Vector2 GetRandomPointInArea();
        bool IsPointInside(Vector2 point);
    }

}