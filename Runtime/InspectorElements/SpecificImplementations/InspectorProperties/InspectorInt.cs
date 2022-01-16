using System; 

namespace MUtility
{
public interface IInspectorInt : IInspectorProperty<int>
{
	bool TryGetRange(object parentObject,  out int min, out int max);
}

[Serializable]public abstract class IntProperty<TParentObject> : InspectorProperty<TParentObject, int>, IInspectorInt
{ 
	public bool TryGetRange(object parentObject, out int min, out int max) => 
		TryGetRange((TParentObject) parentObject, out min, out max);
	
	protected virtual bool TryGetRange(TParentObject container, out int min, out int max)
	{
		min = default;
		max = default;
		return false;
	}
	
	public static implicit operator int(IntProperty<TParentObject> obj) => obj.GetValue(obj.ParentObject);
}

[Serializable] public class IntProperty : IntProperty<object> { }
}