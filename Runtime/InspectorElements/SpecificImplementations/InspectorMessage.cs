using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
public enum InspectorMessageType
{
	Info,
	Warning,
	Error,
	None
}

public interface IInspectorMessage
{
	IEnumerable<string> GetLines(object parentObject);
	InspectorMessageType MessageType(object parentObject);
	bool IsBoxed(object parentObject);
	int FontSize { get; } 
}

[Serializable] public abstract class InspectorMessage<TParentObject> : InspectorElement<TParentObject>, IInspectorMessage
{
	public Func<TParentObject, string> messageGetter;
	public Func<TParentObject, InspectorMessageType> messageTypeGetter;
	public Func<TParentObject, bool> isBoxedGetter;
	public Func<int> fontSizeGetter;

	public IEnumerable<string> GetLines(object parentObject) => GetLines((TParentObject) parentObject);
	public InspectorMessageType MessageType(object parentObject) => MessageType((TParentObject) parentObject);
	public bool IsBoxed(object parentObject)=> IsBoxed((TParentObject) parentObject);
	public virtual int FontSize  => fontSizeGetter?.Invoke() ?? 11;
	protected virtual IEnumerable<string> GetLines(TParentObject parentObject)
	{ 
		string message = messageGetter?.Invoke(parentObject); 
		if (string.IsNullOrEmpty(message))
		{
			yield return "Here should be some message, You forgot to add!";
			yield break;
		}

		string[] lines = message.Split('\n');
		foreach (string line in lines)
			yield return line;
	}

	protected virtual InspectorMessageType MessageType(TParentObject parentObject) =>
		messageTypeGetter?.Invoke(parentObject) ?? InspectorMessageType.None;
	
	protected virtual bool IsBoxed(TParentObject parentObject) => isBoxedGetter?.Invoke(parentObject) ?? true;
}

[Serializable] public class InspectorMessage : InspectorMessage<object> {}

}