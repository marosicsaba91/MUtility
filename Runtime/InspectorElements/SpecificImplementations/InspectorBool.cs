public abstract class InspectorBool<TParentObject> : InspectorElement<TParentObject>, IInspectorProperty<bool>
{
	public bool GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, bool value) => SetValue((TParentObject)  parentObject, value); 
 

	protected abstract bool GetValue(TParentObject parentObject);
	protected virtual void SetValue(TParentObject parentObject, bool value) { } 
}