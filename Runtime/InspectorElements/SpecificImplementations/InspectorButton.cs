using System;

namespace MUtility
{
public interface IInspectorButton
{
	void OnClick(object parentObject);
	string WarningMessage(object parentObject);
}

[Serializable]
public abstract class InspectorButton<TParentObject> : InspectorElement<TParentObject>, IInspectorButton
{
	public delegate void Clicked(TParentObject parentObject);
	public Clicked onClicked;
	public void OnClick(object parentObject) => OnClick((TParentObject) parentObject);

	public string WarningMessage(object parentObject) => WarningMessage((TParentObject) parentObject);
	protected virtual void OnClick(TParentObject parentObject) => onClicked?.Invoke(parentObject);
	protected virtual string WarningMessage(TParentObject parentObject) => null;
}

[Serializable]
public class InspectorButton : InspectorButton<object>
{
}
}