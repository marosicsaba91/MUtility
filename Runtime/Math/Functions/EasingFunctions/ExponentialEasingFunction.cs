using System;
using UnityEngine;

[Serializable]
public class ExponentialEasingFunction : EasingFunctionBase
{
    [Min(1)] public float exponent = 2;
    [SerializeField] CurvePreview curvePreview; 

    protected override float EaseIn01Evaluate(float t) =>
        1 - Mathf.Pow(1 - t, exponent);
}