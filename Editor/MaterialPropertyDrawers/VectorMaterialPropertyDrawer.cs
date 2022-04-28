/*
using UnityEngine;
using UnityEditor;
using System;

namespace MarosiUtility
{

public abstract class VectorMaterialPropertyDrawer : MaterialPropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
    {
        // Setup
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;
        
        // Show the toggle control
        EditorGUIUtility.labelWidth = EditorHelper.LabelWidth - EditorGUIUtility.standardVerticalSpacing;
        Vector4 newValue = DrawVector(position, label, prop.vectorValue);

        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            // Set the new value if it has changed
            prop.vectorValue = newValue;
        }
    }

    protected abstract Vector4 DrawVector(Rect position, string label, Vector4 propVectorValue); 
}

public class Vector2Drawer : VectorMaterialPropertyDrawer
{
    protected override Vector4 DrawVector(Rect position, string label, Vector4 propVectorValue) =>
        EditorGUI.Vector2Field(position, label, propVectorValue); 
}

public class Vector3Drawer :  VectorMaterialPropertyDrawer
{
    protected override Vector4 DrawVector(Rect position, string label, Vector4 propVectorValue) =>
        EditorGUI.Vector3Field(position, label, propVectorValue); 
}
public class Vector4Drawer :  VectorMaterialPropertyDrawer
{
    protected override Vector4 DrawVector(Rect position, string label, Vector4 propVectorValue) =>
        EditorGUI.Vector4Field(position, label, propVectorValue); 
}
public class Vector2IntDrawer : VectorMaterialPropertyDrawer
{
    protected override Vector4 DrawVector(Rect position, string label, Vector4 propVectorValue) =>
        (Vector2) EditorGUI.Vector2IntField(position, label, 
            new Vector2Int((int)propVectorValue.x,(int)propVectorValue.y ));
}

public class Vector3IntDrawer :  VectorMaterialPropertyDrawer
{
    protected override Vector4 DrawVector(Rect position, string label, Vector4 propVectorValue) =>
        (Vector3) EditorGUI.Vector3IntField(position, label, 
            new Vector3Int((int)propVectorValue.x,(int)propVectorValue.y,(int)propVectorValue.z ));
} 
}
*/