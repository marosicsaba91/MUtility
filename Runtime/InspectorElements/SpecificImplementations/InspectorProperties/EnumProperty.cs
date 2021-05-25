using System;
using System.Collections.Generic;
using System.Linq;

namespace MUtility
{ 

[Serializable]
public abstract class EnumProperty<TParentObject, TEnumType> :
    InspectorProperty<TParentObject, TEnumType>, IInspectorProperty<Enum> where TEnumType : Enum
{ 

    public new Enum GetValue(object parentObject) => base.GetValue(parentObject);

    public void SetValue(object parentObject, Enum value) => base.SetValue(parentObject, (TEnumType) value);
    public new IList<Enum> PopupElements(object parentObject) => base.PopupElements(parentObject).Cast<Enum>().ToList();
 
    protected override IList<TEnumType> PopupElements(TParentObject container) =>
        Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>().ToList();
}

[Serializable] public abstract class EnumProperty<TEnumType> : EnumProperty<object, TEnumType>
    where TEnumType : Enum
{
}
}