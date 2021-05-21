using UnityEngine;

namespace MUtility
{
public interface IInspectorElement
{ 
    object ParentObject { get; set; } 
    string Text(object parentObject, string originalLabel);
    Color GetColor(object parentObject);
    bool IsEnabled(object parentObject);
    bool IsVisible(object parentObject);
    Object[] ChangeableObjects(object parentObject);
}

public abstract class InspectorElement<TContainer> : IInspectorElement
{
    public object ParentObject { get; internal set; }
    object IInspectorElement.ParentObject
    {
        get => ParentObject;
        set => ParentObject = value;
    }
 
    public string Text(object parentObject, string originalLabel) => Text((TContainer) parentObject, originalLabel);
    public Color GetColor(object parentObject) => GetColor((TContainer) parentObject);
    public bool IsEnabled(object parentObject) => IsEnabled((TContainer) parentObject);
    public bool IsVisible(object parentObject) => IsVisible((TContainer) parentObject);
    public Object[] ChangeableObjects(object parentObject) => ChangeableObjects((TContainer) parentObject);

    protected virtual string Text(TContainer parentObject, string originalLabel) => null;
    protected virtual Color GetColor(TContainer parentObject) => Color.white;
    protected virtual bool IsEnabled(TContainer parentObject) => true;
    protected virtual bool IsVisible(TContainer parentObject) => true;
    protected virtual Object[] ChangeableObjects(TContainer parentObject) => null;
}
}