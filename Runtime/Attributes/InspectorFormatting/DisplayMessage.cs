using System;

namespace MUtility
{

	public enum MessageType
	{
		Info,
		Warning,
		Error,
		None,
	}

	public enum MessageFormat
	{
		FullLength,
		FullLengthUnboxed,

		ContentLength,
		ContentLengthUnboxed,

		ContentLengthWithTitle,
		ContentLengthWithTitleUnboxed
	}

	public enum MessageSize
	{
		Small,
		Normal,
		Big,
		Huge
	}

	[Serializable]
	public class DisplayMessage
	{
		const MessageType defaultMessageType = MessageType.Info;
		const MessageFormat defaultMessagePosition = MessageFormat.FullLength;
		const MessageSize defaultFontSize = MessageSize.Normal;

		[NonSerialized] public MessageType messageType;
		[NonSerialized] public MessageFormat messageFormat;
		[NonSerialized] public MessageSize messageSize;

		public readonly string textProviderMember;
		public readonly string text;

		Func<object, string> _textGetter;

		public DisplayMessage(string text, bool useTextAsGetterMemberName = false)
		{
			if (useTextAsGetterMemberName)
				textProviderMember = text;
			else
				this.text = text;
		}

		public int FontSize
		{
			get
			{
				switch (messageSize)
				{
					case MessageSize.Small:
						return 10;
					case MessageSize.Normal:
						return 12;
					case MessageSize.Big:
						return 16;
					case MessageSize.Huge:
						return 22;
					default:
						return 12;
				}
			}
		}

		public bool IsBoxed
		{
			get
			{
				switch (messageFormat)
				{
					case MessageFormat.FullLengthUnboxed:
					case MessageFormat.ContentLengthUnboxed:
					case MessageFormat.ContentLengthWithTitleUnboxed:
						return false;
				}
				return true;
			}
		}

		public bool IsFullLength
		{
			get
			{
				switch (messageFormat)
				{
					case MessageFormat.FullLength:
					case MessageFormat.FullLengthUnboxed:
						return true;
				}
				return false;
			}
		}

		public bool ShowTitle
		{
			get
			{
				switch (messageFormat)
				{
					case MessageFormat.ContentLengthWithTitle:
					case MessageFormat.ContentLengthWithTitleUnboxed:
						return true;
				}
				return false;
			}
		}

		public void Initialise(object owner)
		{
#if UNITY_EDITOR
			if (textProviderMember.NotNullOrEmpty() && _textGetter == null)
				InspectorDrawingUtility.TryGetAGetterFromMember(owner.GetType(), textProviderMember, out _textGetter);
#endif
		}

		public string[] GetLines(object owner)
		{
			string t = _textGetter != null ? _textGetter.Invoke(owner) : text;

			if (t.IsNullOrEmpty())
				return Array.Empty<string>();

			return t.Split('\n');
		}
	}
}