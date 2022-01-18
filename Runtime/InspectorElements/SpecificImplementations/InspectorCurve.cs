using System;
using UnityEngine;

namespace MUtility
{
public interface IInspectorCurve
{
    float Evaluate(object parentObject, float time);
    Rect DefaultArea(object parentObject); 
    Color GetCurveColor(object parentObject);
    
    float Height(object parentObject);
}


public abstract class InspectorCurve<TParentObject> : InspectorElement<TParentObject>, IInspectorCurve
{
    public const float defaultCurveHeight = 150f;
    
    public delegate float EvaluateFunction(TParentObject parentObject, float time);
    public EvaluateFunction evaluateFunction;
    public delegate Rect RectGetter(TParentObject parentObject);
    public RectGetter getDefaultRect;
    public delegate float FloatGetter(TParentObject parentObject);
    public FloatGetter getUIElementHeight;
    public ColorGetter getCurveColor;
    
    
    public float Evaluate(object parentObject, float time) => Evaluate((TParentObject) parentObject, time);
    public Rect DefaultArea(object parentObject) => DefaultArea((TParentObject) parentObject);

    public Color GetCurveColor(object parentObject) => GetCurveColor((TParentObject) parentObject);

    public float Height(object parentObject) => Height((TParentObject) parentObject);

    protected virtual float Evaluate(TParentObject parentObject, float time) =>
        evaluateFunction?.Invoke(parentObject, time) ?? 0;

    protected virtual Rect DefaultArea(TParentObject parentObject) =>
        getDefaultRect?.Invoke(parentObject) ?? new Rect(x: -1, y: -1, width: 2, height: 2);
    protected virtual float Height(TParentObject parentObject) => 
        getUIElementHeight?.Invoke(parentObject) ?? defaultCurveHeight;
    protected virtual Color GetCurveColor(TParentObject parentObject)
    {
        if (getCurveColor != null)
            return getCurveColor(parentObject);
        
#if UNITY_EDITOR
        return EditorHelper.functionColor;
#else 
        return Color.yellow; 
#endif
    } 
    
}

[Serializable]
public class InspectorCurve : InspectorCurve<object> { }
}