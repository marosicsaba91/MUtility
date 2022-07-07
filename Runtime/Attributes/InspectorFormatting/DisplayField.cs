using System;
using UnityEngine;

[Serializable]
public class DisplayField
{
    [NonSerialized] public string memberName;
    public float functionZoom = 1;
    public Vector2 functionOffset = Vector2.zero;

    public DisplayField(string memberName) =>
       this.memberName = memberName;
    
}
