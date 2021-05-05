public interface IInspectorButton
{
	void OnClick(object parentObject);
	string WarningMessage(object parentObject);
}

public abstract class InspectorButton<TParentObject> : InspectorElement<TParentObject>, IInspectorButton
{
	public void OnClick(object parentObject) => OnClick((TParentObject) parentObject);
	public string WarningMessage(object parentObject) => WarningMessage((TParentObject) parentObject);
	protected abstract void OnClick(TParentObject parentObject);
	protected virtual string WarningMessage(TParentObject parentObject) => null; 
}