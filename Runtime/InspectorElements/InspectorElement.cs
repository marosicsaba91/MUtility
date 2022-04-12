using UnityEngine;

namespace MUtility
{
public interface IInspectorElement
{ 
    object ParentObject { get; set; } 
    string GetLabel(object parentObject, string originalLabel);
    Color GetColor(object parentObject);
    bool IsEnabled(object parentObject);
    bool IsVisible(object parentObject);
    Object[] ChangeableObjects(object parentObject);
}

public abstract class InspectorElement<TParentObject> : IInspectorElement
{
    public delegate Color ColorGetter(TParentObject parentObject);
    public delegate bool BoolGetter(TParentObject parentObject);
    public delegate string StringGetter(TParentObject parentObject);
     
    public ColorGetter getColor;
    public StringGetter getLabel;
    public BoolGetter isEnabled;
    public BoolGetter isVisible;
    
    public object ParentObject { get; internal set; }
    object IInspectorElement.ParentObject
    {
        get => ParentObject;
        set => ParentObject = value;
    }
    
    public void Setup(TParentObject parent)
    {
        ParentObject = parent;
    }
 
    public string GetLabel(object parentObject, string originalLabel) => GetLabel((TParentObject) parentObject, originalLabel);
    public Color GetColor(object parentObject) => GetColor((TParentObject) parentObject);
    public bool IsEnabled(object parentObject) => IsEnabled((TParentObject) parentObject);
    public bool IsVisible(object parentObject) => IsVisible((TParentObject) parentObject);
    public Object[] ChangeableObjects(object parentObject) => ChangeableObjects((TParentObject) parentObject);

    protected virtual string GetLabel(TParentObject parentObject, string originalLabel) => getLabel?.Invoke(parentObject);

    protected virtual Color GetColor(TParentObject parentObject) => getColor?.Invoke(parentObject) ?? Color.white; 
    protected virtual bool IsEnabled(TParentObject parentObject) => isEnabled?.Invoke(parentObject) ?? true; 
    protected virtual bool IsVisible(TParentObject parentObject) => isVisible?.Invoke(parentObject) ?? true; 
    protected virtual Object[] ChangeableObjects(TParentObject parentObject) => null;
}
}