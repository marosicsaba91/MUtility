
using UnityEngine;

public abstract class InspectorVector2<TParentObject> : InspectorElement<TParentObject>, IInspectorProperty<Vector2>
{
	public Vector2 GetValue(object parentObject) => GetValue((TParentObject) parentObject);
	public void SetValue(object parentObject, Vector2 value) => SetValue((TParentObject)  parentObject, value); 
 

	protected abstract Vector2 GetValue(TParentObject parentObject);
	protected virtual void SetValue(TParentObject parentObject, Vector2 value) { } 
}