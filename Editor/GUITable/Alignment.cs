#if UNITY_EDITOR
using System;
using UnityEngine;

namespace MUtility
{
public enum Alignment
{
    Left,
    Center,
    Right
}

public static class AlignmentHelper
{
    const int padding = 5;

    public static GUIStyle ToGUIStyle(this Alignment alignment, int fontSize = 12, FontStyle fontStyle  = FontStyle.Normal)
    { 
        var result = new GUIStyle(GUI.skin.label) 
            {
                alignment = alignment.ToTextAnchor(),
                fontSize = fontSize,
                fontStyle = fontStyle
            };

        if (alignment != Alignment.Center)
            result.padding = new RectOffset(padding, padding, 0, 0);

        return result;
    }

    public static TextAnchor ToTextAnchor(this Alignment alignment) => alignment switch
    {
        Alignment.Left => TextAnchor.MiddleLeft,
        Alignment.Center => TextAnchor.MiddleCenter,
        Alignment.Right => TextAnchor.MiddleRight,
        _ => throw new ArgumentOutOfRangeException()
    };
}
}
#endif