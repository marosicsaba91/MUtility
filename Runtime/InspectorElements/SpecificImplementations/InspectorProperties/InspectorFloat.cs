using System;

namespace MUtility
{
public interface IFloatProperty : IInspectorProperty<float>
{
	bool TryGetMin(object parentObject, out float min);
	bool TryGetMax(object parentObject, out float max);
}

[Serializable]
public abstract class InspectorFloat<TParentObject> : InspectorProperty<TParentObject, float>, IFloatProperty
{
	public delegate float FloatGetter(TParentObject parentObject);
	public FloatGetter getMinimum;
	public FloatGetter getMaximum;
	
	public bool TryGetMin(object parentObject, out float min) =>
		TryGetMin((TParentObject) parentObject, out min);
 
	public bool TryGetMax(object parentObject, out float max) =>
		TryGetMax((TParentObject) parentObject,  out max);

	protected virtual bool TryGetMin(TParentObject container, out float min)
	{
		min = getMinimum?.Invoke(container) ?? 0; 
		return getMinimum != null;
	}
	protected virtual bool TryGetMax(TParentObject container, out float max)
	{
		max = getMaximum?.Invoke(container) ?? 0; 
		return getMaximum != null;
	}
}

[Serializable]
public class InspectorFloat : InspectorFloat<object>
{
}

}