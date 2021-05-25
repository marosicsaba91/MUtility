#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorUnityObject), useForChildren: true)]
public class InspectorUnityObjectDrawer :  InspectorPropertyDrawer<Object, IInspectorUnityObject>
{

	protected override Object GetValue(
		Rect position, GUIContent label, Object oldValue,
		IInspectorUnityObject inspectorProperty, object parentObject) =>
		
		EditorGUI.ObjectField(position, label, oldValue, inspectorProperty.ContentType, allowSceneObjects: true);

}
}
#endif