using System.Collections.Generic;

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
public abstract class InspectorMessage<TParentObject> : InspectorElement<TParentObject>, IInspectorMessage
{
	
	public IEnumerable<string> GetLines(object parentObject) => GetLines((TParentObject) parentObject);
	public InspectorMessageType MessageType(object parentObject) => MessageType((TParentObject) parentObject);
	public bool IsBoxed(object parentObject)=> IsBoxed((TParentObject) parentObject);
	public virtual int FontSize => 10;
	protected abstract IEnumerable<string> GetLines(TParentObject parentObject);
	protected virtual InspectorMessageType MessageType(TParentObject parentObject) => InspectorMessageType.None;
	protected virtual bool IsBoxed(TParentObject parentObject) => true;

}
}