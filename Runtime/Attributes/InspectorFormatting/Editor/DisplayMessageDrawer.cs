#if UNITY_EDITOR

using System.Text;
using UnityEditor;
using UnityEngine;

namespace MUtility
{
	[CustomPropertyDrawer(typeof(DisplayMessage), useForChildren: true)]
	public class DisplayMessageDrawer : PropertyDrawer
	{
		DisplayMessage _message;
		string[] _lines;
		bool _initialised = false;
		object _owner;


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			_owner = property.GetObjectWithProperty();
			_message = (DisplayMessage)property.GetObjectOfProperty();
			if (!_initialised)
			{
				_message?.Initialise(_owner);
				_owner = property.GetObjectWithProperty();
			}

			_lines = _message.GetLines(_owner);
			int messageLineCount = _lines.Length;

			if (messageLineCount == 0)
				return 0;
			int fontSize = _message.FontSize;
			bool boxed = _message.IsBoxed;
			int spacingSize = Mathf.RoundToInt(0.2f * fontSize);
			const float minimumBoxedHeight = 22;

			float h = messageLineCount * (fontSize + spacingSize) + (boxed ? 7 : 0);

			if (boxed)
				return Mathf.Max(h, minimumBoxedHeight);
			return h;

		}

		internal static GUIContent GetHelpIcon(UnityEditor.MessageType type, bool small)
		{
			string t = string.Empty;

			switch (type)
			{
				case UnityEditor.MessageType.Info:
					t += "console.infoicon";
					break;
				case UnityEditor.MessageType.Warning:
					t += "console.warnicon";
					break;
				case UnityEditor.MessageType.Error:
					t += "console.erroricon";
					break;
				default:
					return new GUIContent();
			}

			t += small ? ".sml" : string.Empty;
			return EditorGUIUtility.IconContent(t);
		}

		static UnityEditor.MessageType ToEditorMessageType(MessageType messageType)
		{
			switch (messageType)
			{
				case MessageType.Info:
					return UnityEditor.MessageType.Info;
				case MessageType.Warning:
					return UnityEditor.MessageType.Warning;
				case MessageType.Error:
					return UnityEditor.MessageType.Error;
				default:
					return UnityEditor.MessageType.None;
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (_lines.Length == 0)
				return;
			StringBuilder messageBuilder = new StringBuilder();

			int i = 0;
			foreach (string line in _lines)
			{
				if (i != 0)
					messageBuilder.Append("\n");
				messageBuilder.Append(line);
				i++;
			}

			MessageType messageType = _message.messageType;
			UnityEditor.MessageType editorMessageType = ToEditorMessageType(messageType);
			GUIContent content = GetHelpIcon(editorMessageType, position.height < 40);
			content.text = messageBuilder.ToString();
			int fontSize = _message.FontSize;

			GUIStyle style = _message.IsBoxed
				? new GUIStyle(EditorStyles.helpBox) { fontSize = fontSize }
				: new GUIStyle(EditorStyles.label) { fontSize = fontSize };

			Rect contentPosition = _message.IsFullLength ? position : EditorHelper.ContentRect(position);
			GUI.Label(contentPosition, content, style);

			if (_message.ShowTitle)
			{
				Rect labelPosition = EditorHelper.LabelRect(position);
				GUI.Label(labelPosition, label);
			}
		}
	}
}
#endif