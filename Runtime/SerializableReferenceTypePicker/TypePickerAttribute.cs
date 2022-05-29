using UnityEngine;
 
public class TypePickerAttribute : PropertyAttribute
{
    public bool forceSmall = false;

    public TypePickerAttribute(bool forceSmall)
    {
        this.forceSmall = forceSmall;
    }  
    public TypePickerAttribute()
    {
    }
} 