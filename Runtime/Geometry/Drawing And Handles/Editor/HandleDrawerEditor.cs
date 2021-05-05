#if UNITY_EDITOR
using Example.Geometry;
using UnityEditor;

[CustomEditor(typeof(GizmoAndHandleDrawer))]
public class HandleDrawerEditor : Editor
{
    void OnSceneGUI() =>
        ((GizmoAndHandleDrawer)target).ApplyHandles( );
}
#endif