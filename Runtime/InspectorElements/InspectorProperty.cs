using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
    public interface IInspectorProperty<T>
    {
        T GetValue(object parentObject);
        void SetValue(object parentObject, T newValue);

        IList<T> PopupElements(object parentObject);
    }


    [Serializable]
    public class InspectorProperty<TParentObject, TPropertyType> :
        InspectorElement<TParentObject>, IInspectorProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType value;
        public delegate TPropertyType ValueGetter(TParentObject parentObject);
        public delegate TPropertyType ValueSetter(TParentObject parentObject, TPropertyType inputValue);
        public ValueGetter valueGetter;
        public ValueSetter valueSetter;

        public delegate void ValueChangedDelegate(TParentObject parent, TPropertyType oldValue, TPropertyType newValue);

        public ValueChangedDelegate valueChanged;
        public TPropertyType GetValue(object parentObject) => 
            parentObject ==null ? default : GetValue((TParentObject) parentObject);

        public void SetValue(object parentObject, TPropertyType newValue)
        {
            if (parentObject == null) return;
            if (Equals(value, newValue)) return;
            TPropertyType oldValue = value;
            SetValue((TParentObject) parentObject, newValue);
            valueChanged?.Invoke((TParentObject) parentObject, oldValue, newValue);
        }

        public IList<TPropertyType> PopupElements(object parentObject) => PopupElements((TParentObject) parentObject);

        public TPropertyType Value
        {
            get => GetValue(ParentObject);
            set => SetValue(ParentObject, value);
        }

        protected virtual TPropertyType GetValue(TParentObject parentObject) =>
            valueGetter == null? value : valueGetter.Invoke(parentObject);

        protected virtual void SetValue(TParentObject parentObject, TPropertyType newValue) =>
            value = valueSetter == null ? newValue : valueSetter.Invoke(parentObject, newValue);
        
        protected virtual IList<TPropertyType> PopupElements(TParentObject container) => null;

        public static implicit operator TPropertyType(InspectorProperty<TParentObject, TPropertyType> obj) =>
            obj.GetValue(obj.ParentObject);
    }


}
    

