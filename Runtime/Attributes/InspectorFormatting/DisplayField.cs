using System;
using UnityEngine;

[Serializable]
public class InspectorMember
{
    readonly string _memberName; // DON'T CREATE PROPERTY OUT OF THIS;
    public float functionZoom = 1;
    public Vector2 functionOffset = Vector2.zero;
    public string GetMemberName() => _memberName;  // DON'T CREATE PROPERTY OUT OF THIS;

    public InspectorMember(string memberName) =>
        _memberName = memberName;
    
}
