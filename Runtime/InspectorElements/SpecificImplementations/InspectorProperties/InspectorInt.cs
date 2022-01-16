using System; 

namespace MUtility
{
public interface IInspectorInt : IInspectorProperty<int>
{
	bool TryGetMin(object parentObject, out int min);
	bool TryGetMax(object parentObject, out int max);
}

[Serializable]public abstract class InspectorInt<TParentObject> : InspectorProperty<TParentObject, int>, IInspectorInt
{ 
	public delegate int IntGetter(TParentObject parentObject);
	public IntGetter getValue;
	public IntGetter getMinimum;
	public IntGetter getMaximum;
	
	public bool TryGetMin(object parentObject, out int min) =>
		TryGetMin((TParentObject) parentObject, out min);
 
	public bool TryGetMax(object parentObject, out int max) =>
		TryGetMax((TParentObject) parentObject,  out max);

	protected virtual bool TryGetMin(TParentObject container, out int min)
	{
		min = getMinimum?.Invoke(container) ?? 0; 
		return getMinimum != null;
	}
	protected virtual bool TryGetMax(TParentObject container, out int max)
	{
		max = getMaximum?.Invoke(container) ?? 0; 
		return getMaximum != null;
	}
	
	public static implicit operator int(InspectorInt<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}

[Serializable] public class InspectorInt : InspectorInt<object> { }
}