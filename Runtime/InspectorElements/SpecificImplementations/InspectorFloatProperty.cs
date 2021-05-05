
public interface IInspectorFloatProperty : IInspectorProperty<float> { 
	bool TryGetRange(object parentObject, out float min, out float max); 
}

public abstract class InspectorFloatProperty<TParentObject> : InspectorElement<TParentObject>, IInspectorFloatProperty
{
	public float GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, float value) => SetValue((TParentObject)  parentObject, value);
	public bool TryGetRange(object parentObject, out float min, out float max) => 
		TryGetRange((TParentObject) parentObject,  out min, out max);
 

	protected abstract float GetValue(TParentObject parentObject);
	protected virtual void SetValue(TParentObject parentObject, float value) { }

	protected virtual bool TryGetRange(TParentObject container, out float min, out float max)
	{
		min = 0;
		max = 0;
		return false;
	}
}