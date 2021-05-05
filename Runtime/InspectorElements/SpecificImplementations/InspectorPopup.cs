using System.Collections.Generic;

public interface IInspectorPopup 
{
    IEnumerable<object> Elements(object parentObject);
    int GetSelectedElement(object parentObject);
    void SetSelectedElement(object parentObject, int index);
}

public abstract class InspectorPopup<TParentObject> : InspectorElement<TParentObject>, IInspectorPopup
{
    public IEnumerable<object> Elements(object parentObject) => Elements((TParentObject) parentObject);
    public int GetSelectedElement(object parentObject) => GetSelectedElement((TParentObject) parentObject);
    public void SetSelectedElement(object parentObject, int index) => SetSelectedElement((TParentObject) parentObject, index);
    
    protected abstract IEnumerable<object> Elements(TParentObject parentObject);
    public abstract int GetSelectedElement(TParentObject parentObject);
    public abstract void SetSelectedElement(TParentObject parentObject, int index);
}
