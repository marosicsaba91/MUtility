using System;
using UnityEngine;

namespace MUtility
{
[Serializable]
public class SinusoidalEasingFunction : EasingFunctionBase
{
    [SerializeField] CurvePreview curvePreview;
    protected override float EaseIn01Evaluate(float t) => Mathf.Sin(t * Mathf.PI * 0.5f);
}
}