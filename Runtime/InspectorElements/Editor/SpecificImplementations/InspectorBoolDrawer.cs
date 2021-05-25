#if UNITY_EDITOR
using UnityEditor;
using UnityEngine; 

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<bool>), useForChildren: true)]
public class InspectorBoolDrawer : InspectorPropertyDrawer<bool, IInspectorProperty<bool>>
{
    protected override bool GetValue(
        Rect position, GUIContent label, bool oldValue, 
        IInspectorProperty<bool> inspectorProperty, object parentObject) =>
        
        EditorGUI.Toggle(position, label, oldValue);
}
}
#endif
