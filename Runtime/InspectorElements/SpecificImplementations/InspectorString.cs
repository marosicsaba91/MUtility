namespace MUtility
{
public abstract class InspectorString<TParentObject> : InspectorElement<TParentObject>,  IInspectorProperty<string>
{
	public string GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, string value) => SetValue((TParentObject)  parentObject, value); 

	protected abstract string GetValue(TParentObject parentObject);
	protected virtual void SetValue(TParentObject parentObject, string value) { } 
}
}