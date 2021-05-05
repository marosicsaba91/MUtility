public interface IInspectorProperty<T>{
    T GetValue(object parentObject); 
    void SetValue(object parentObject, T value);
}