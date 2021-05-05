#if UNITY_EDITOR
using UnityEditor;

namespace MUtility
{
[CustomEditor(typeof(GizmoAndHandleDrawer))]
public class HandleDrawerEditor : Editor
{
    void OnSceneGUI() =>
        ((GizmoAndHandleDrawer)target).ApplyHandles( );
}
}
#endif