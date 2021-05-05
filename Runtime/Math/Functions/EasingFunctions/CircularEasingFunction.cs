using System;
using UnityEngine;

[Serializable]
public class CircularEasingFunction : EasingFunctionBase
{
    [SerializeField] CurvePreview curvePreview;

    protected override float EaseIn01Evaluate(float t) =>
        Mathf.Sqrt(1 - ((1 - t) * (1 - t)));

}