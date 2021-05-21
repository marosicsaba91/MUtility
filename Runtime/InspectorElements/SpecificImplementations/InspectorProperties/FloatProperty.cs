using System;

namespace MUtility
{
public interface IFloatProperty : IInspectorProperty<float>
{
	bool TryGetRange(object parentObject, out float min, out float max);
}

[Serializable]
public abstract class FloatProperty<TParentObject> : InspectorProperty<TParentObject, float>, IFloatProperty
{
	public bool TryGetRange(object parentObject, out float min, out float max) =>
		TryGetRange((TParentObject) parentObject, out min, out max);

	protected virtual bool TryGetRange(TParentObject container, out float min, out float max)
	{
		min = 0;
		max = 0;
		return false;
	}
}

[Serializable]
public class FloatProperty : FloatProperty<object>
{
}

}