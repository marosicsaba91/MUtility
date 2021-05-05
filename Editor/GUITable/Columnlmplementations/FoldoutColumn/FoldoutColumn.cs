#if UNITY_EDITOR
using System; 
using UnityEditor;
using UnityEngine;
using Utility;

namespace EditorGUITable
{
public abstract class FoldoutColumn<T>: Column<FoldableRow<T>>
{
    public static bool inArray = false;
    protected FoldoutColumn(ColumnInfo columnInfo = null) : base(columnInfo) { }
    
    public override void DrawCell(Rect position, FoldableRow<T> row, GUIStyle style, Action onChanged)
    {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        position = DrawFoldout(position, row);
        DrawContent(position, row, style, onChanged);
        EditorGUI.indentLevel = indent;
    }

    public abstract void DrawContent(Rect position, FoldableRow<T> row, GUIStyle style, Action onChanged);

    static Rect DrawFoldout(Rect position, FoldableRow<T> row)
    {
        const float indentW = 14;
        const float dropdownW = 14;
        float minimumW = EditorHelper.LabelStartX - 16;
         
        float fullW = position.width;
        float shift = minimumW + row.level * indentW;
        position.x += shift;
        position.width = dropdownW;
        if (inArray) position.x += dropdownW;
        if (row.ChildCount > 0)
        {
            bool newOpened = EditorGUI.Foldout(
                position,
                row.isRowOpen,
                GUIContent.none,
                FoldoutStyle);
            if(newOpened!= row.isRowOpen)
                row.OnFold(newOpened, Event.current.alt);
        }
        if (inArray) position.x -= dropdownW;
        
        position.x += dropdownW;
        position.width = fullW - dropdownW - shift;

        return position;
    }

    protected override GUIStyle GetDefaultStyle() => new GUIStyle(GUI.skin.label);

    static GUIStyle FoldoutStyle => new GUIStyle(EditorStyles.foldout)
    {   
        alignment = TextAnchor.MiddleLeft
    };
}
}
#endif