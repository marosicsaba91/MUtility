#if UNITY_EDITOR
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility.SpecificImplementations
{
[CustomPropertyDrawer(typeof(IInspectorMessage), useForChildren: true)]
public class InspectorMessageDrawer : InspectorElementDrawer
{
    public override bool Draw(
        Rect position,
        IInspectorElement inspectorElement,
        SerializedProperty property,
        object parentObject,
        Object serializedObject,
        GUIContent label)
    {
        var message = (IInspectorMessage) inspectorElement;
        string[] lines = message.GetLines(parentObject).ToArray();
        if (lines.Length == 0) return false;
        var messageBuilder = new StringBuilder();

        var i = 0;
        foreach (string line in lines)
        {
            if (i != 0) messageBuilder.Append("\n");
            messageBuilder.Append(line);
            i++;
        }

        InspectorMessageType inspectorMessageType = message.MessageType(parentObject);
        MessageType editorMessageType = ToEditorMessageType(inspectorMessageType);
        GUIContent content = GetHelpIcon(editorMessageType);
        content.text = messageBuilder.ToString();
        int fontSize = message.FontSize;

        GUIStyle style = message.IsBoxed(parentObject)
            ? new GUIStyle(EditorStyles.helpBox) {fontSize = fontSize}
            : new GUIStyle(EditorStyles.label) {fontSize = fontSize};
        
        GUI.Label(position, content, style);

        return false;
    }

    internal static GUIContent GetHelpIcon(MessageType type)
    {
        switch (type)
        {
            case MessageType.Info:
                return EditorGUIUtility.IconContent("console.infoicon");
            case MessageType.Warning:
                return EditorGUIUtility.IconContent("console.warnicon");
            case MessageType.Error:
                return EditorGUIUtility.IconContent("console.erroricon");
            default:
                return new GUIContent();
        }
    }

    static MessageType ToEditorMessageType(InspectorMessageType inspectorMessageType)
    {
        switch (inspectorMessageType)
        {
            case InspectorMessageType.Info:
                return MessageType.Info;
            case InspectorMessageType.Warning:
                return MessageType.Warning;
            case InspectorMessageType.Error:
                return MessageType.Error;
            default:
                return MessageType.None;
        }
    }

    public override float? GetPropertyHeight(
        IInspectorElement inspectorElement,
        SerializedProperty property, 
        object parentObject,
        Object serializedObject, GUIContent label)
    {
        var message = (IInspectorMessage) inspectorElement;
        int messageCount = message.GetLines(parentObject).Count();
        if (messageCount == 0) return 0;
        int fontSize = message.FontSize;
        bool boxed = message.IsBoxed(parentObject);
        int spacingSize = Mathf.RoundToInt(0.2f * fontSize);

        float h = messageCount * (fontSize + spacingSize) + (boxed ? 7 : 0);
        const float minimumBoxedHeight = 38;
        if(boxed)
            return Mathf.Max(h, minimumBoxedHeight);
        return h;
    }
}
}
#endif