using UnityEngine;

namespace MUtility
{
public interface IInspectorCurve
{
    abstract float Evaluate(object parentObject, float time);
    abstract Rect DefaultArea(object parentObject); 
    abstract Color GetCurveColor(object parentObject);
    
    abstract float Height(object parentObject);
}


public abstract class InspectorCurve<TContainer> : InspectorElement<TContainer>, IInspectorCurve
{
    public const float defaultCurveHeight = 150f;
    
    public float Evaluate(object parentObject, float time) => Evaluate((TContainer) parentObject, time);
    public Rect DefaultArea(object parentObject) => DefaultArea((TContainer) parentObject);

    public Color GetCurveColor(object parentObject) => GetCurveColor((TContainer) parentObject);

    public float Height(object parentObject) => Height((TContainer) parentObject);

    protected abstract float Evaluate(TContainer parentObject, float time);

    protected virtual Rect DefaultArea(TContainer parentObject) => new Rect(x: -1, y: -1, width: 2, height: 2);
    protected virtual float Height(TContainer parentObject) => defaultCurveHeight;
    protected virtual Color GetCurveColor(TContainer parentObject)
    {
#if UNITY_EDITOR
        return EditorHelper.functionColor;
#endif
#pragma warning disable 162
        return Color.yellow;
#pragma warning restore 162
    } 
}
}