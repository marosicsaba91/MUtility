public interface IInspectorInt : IInspectorProperty<int>
{
	bool TryGetRange(object parentObject,  out int min, out int max);
}

public abstract class InspectorInt<TParentObject> : InspectorElement<TParentObject>, IInspectorInt
{
	public int GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, int value) => SetValue((TParentObject)  parentObject, value);
	public bool TryGetRange(object parentObject, out int min, out int max) => 
		TryGetRange((TParentObject) parentObject, out min, out max);
 

	protected abstract int GetValue(TParentObject parentObject);
	protected virtual void SetValue(TParentObject parentObject, int value) { }

	protected virtual bool TryGetRange(TParentObject container, out int min, out int max)
	{
		min = default;
		max = default;
		return false;
	}
}