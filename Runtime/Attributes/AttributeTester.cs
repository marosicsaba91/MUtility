using System;
using MUtility;
using UnityEngine;
using Object = UnityEngine.Object;

interface IAnimal
{
    void SaySomething();
}

public class AttributeTester : MonoBehaviour
{
    //[SerializeField] NoesArk ark;
    //[SerializeField] Cat cat;
    //[SerializeField] Dog dog;  
    //[SerializeField] Animal animal;
    //[SerializeReference] Animal referencedAnimal;
    //[SerializeReference] IAnimal iAnimal;
    //[SerializeReference, TypePicker] IAnimal iAnimalWithTypePicker;
    //[SerializeReference, TypePicker] List<IAnimal> animals;

    [Serializable]
    struct TestStruct
    {
        public bool disable; 
        public bool hide; 
        public Color color; 
        [Color(nameof(color))]
        [DisableIf(nameof(disable))]
        [HideIf(nameof(hide))]
        public string switchable;
    }

    [SerializeField] TestStruct testStruct;
    [SerializeField] TestStruct[] testStructs;
    

    [Space]
    [SerializeField] bool showCondition1;
    [SerializeField] bool showCondition2;
    [SerializeField] bool showCondition3;
    [SerializeField] Color fieldColor;
    [SerializeField] Gradient fieldGradient; 
    
    [ShowIf(nameof(showCondition1), nameof(TestConditionalMethod))] 
    [EnableIf( nameof(TestConditionalProperty))] 
    [Color(nameof(FieldColor))]
    [SerializeField, Range(0,1)] float formattedField;
    
    [ValueChangeCallback(nameof(ObjectValueChanged))]
    [SerializeField] Object objectField;

    Color FieldColor => fieldGradient?.Evaluate(formattedField) ?? Color.white;
    
    [ShowIf(nameof(showCondition1), nameof(TestConditionalMethod))]
    [EnableIf( nameof(TestConditionalProperty))]
    [Color( nameof(fieldColor))]
    [SerializeField] DisplayField myButton= new DisplayField(nameof(TestMethod));
    [SerializeField] DisplayField myField = new DisplayField(nameof(formattedField));
    [SerializeField] DisplayField myProperty = new DisplayField(nameof(TestConditionalProperty));
    [Space]
    [Color(nameof(FieldColor))]
    [SerializeField] DisplayField myProperty2 = new DisplayField(nameof(TestStringProperty));
    [SerializeField] DisplayField myProperty3 = new DisplayField(nameof(r));
    [HideIf(nameof(showCondition2))]
    [SerializeField] int a; 
    [ShowIf(nameof(showCondition2))]
    [SerializeField] int c;
    [Color(DisplayColor.Purple)]
    [HideIf(nameof(showCondition2))]
    [SerializeField] int d;
    [DisableIf(nameof(showCondition2))]
    [SerializeField] int e;
    [HideIf(nameof(showCondition2))]
    [SerializeField] int f;
    [SerializeField] RectInt rrri;
    [SerializeField] DisplayField myProperty4 = new DisplayField(nameof(b));
    [SerializeField] DisplayField myProperty5 = new DisplayField(nameof(v));
    [SerializeField] DisplayField functionTest = new DisplayField(nameof(ModSin));

    [Color(DisplayColor.Red)]
    [SerializeField] DisplayMessage message = new DisplayMessage(nameof(GetString), false)
        {   messageType = MessageType.Error, 
            messageFormat = MessageFormat.FullLength,
            messageSize = MessageSize.Huge};

    Direction2D v;
    RectInt r;
    Bounds b;

    
    [SerializeField] float testObject;
    float ModSin(float f)
    {
        return Mathf.Sin(f * formattedField) * testObject;
    }
    
    float AbsValue(float f)
    {
        return Mathf.Abs(f * formattedField) * testObject;
    }   
    string GetString()
    {
        return "AAAAA\nBBBBB\nCCCCC";
    }

    void FloatValueChanged(float oldValue, float newValue)
    {
        Debug.Log($"Value Changed: {oldValue} {newValue}");
    }
    
    void ObjectValueChanged(Object oldValue, Object newValue)
    {
        Debug.Log($"Value Changed: {oldValue} {newValue}");
    }

    bool TestConditionalMethod() => showCondition2;

    bool TestConditionalProperty => showCondition3;
    
    static string st;
    static string TestStringProperty
    {
        get => st;
        set => st = value.Length > 10 ? value.Substring(0, 10) : value;
    }

    void TestMethod()
    {
        Debug.Log("Test Successful");
    }

    [Serializable]
    class Animal : IAnimal
    {
        [SerializeField] string name;

        public virtual void SaySomething()
        {
            Debug.Log($"My name is {name}");
        }
    }

    [Serializable]
    class Dog : Animal
    {
        [SerializeField] bool isGoodBoy = true;
        [SerializeField] int age;

        public void Bark()
        {
            Debug.Log(isGoodBoy ? "Woof" : "I hate You!");
        }

        public override void SaySomething()
        {
            Debug.Log("Woof!");
        }
    }

    [Serializable]
    class Cat : Animal
    { 
        [SerializeField] int livesLeft = 9;
        [SerializeField] int age;
        
        public void Meow()
        {
            Debug.Log($"I have {livesLeft} lives left");
        }
        
        
        public override void SaySomething()
        {
            Debug.Log("Miau!");
        }
    } 
    
    [Serializable]
    class NoesArk 
    {  
        [SerializeReference, TypePicker] Animal animal1;
        [SerializeReference, TypePicker] Animal animal2;
        [SerializeReference, TypePicker] Animal[] moreAnimals;
    }
}
