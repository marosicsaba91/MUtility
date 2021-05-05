using UnityEngine;

namespace MUtility
{
public interface IInspectorElement
{
    string Text(object parentObject);
    Color GetGUIColor(object parentObject);
    bool IsEnabled(object parentObject);
    bool IsVisible(object parentObject);
    Object[] ChangeableObjects(object parentObject);
}

public abstract class InspectorElement<TContainer> : IInspectorElement
{
    public string Text(object parentObject) => Text((TContainer) parentObject);
    public Color GetGUIColor(object parentObject) => GetColor((TContainer) parentObject);
    public bool IsEnabled(object parentObject) => IsEnabled((TContainer) parentObject);
    public bool IsVisible(object parentObject) => IsVisible((TContainer) parentObject);
    public Object[] ChangeableObjects(object parentObject) => ChangeableObjects((TContainer) parentObject);

    protected virtual string Text(TContainer parentObject) => null;
    protected virtual Color GetColor(TContainer parentObject) => Color.white;
    protected virtual bool IsEnabled(TContainer parentObject) => true;
    protected virtual bool IsVisible(TContainer parentObject) => true;
    protected virtual Object[] ChangeableObjects(TContainer parentObject) => null;
}
}