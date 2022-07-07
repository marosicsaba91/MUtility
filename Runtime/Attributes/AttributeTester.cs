using System;
using System.Collections.Generic;
using MUtility;
using UnityEngine;
using Object = UnityEngine.Object;

interface IAnimal
{
    void SaySomething();
}

public class TypePickerTester : MonoBehaviour
{
    [Serializable]
    class MyClass<T>
    { 
        
    }

    [SerializeField] MyClass<int> testt;
    [SerializeField] bool show;
    [SerializeField] bool show1;
    [SerializeField] bool show2;
    [SerializeField, ShowIf(nameof(show),nameof(Showw), nameof(Shooow))] NoesArk ark;
    [SerializeField] Cat cat;
    [SerializeField] Dog dog;  
    [SerializeField] Animal animal;
    [SerializeReference] Animal referencedAnimal;
    [SerializeReference] IAnimal iAnimal;
    [SerializeReference, TypePicker] IAnimal iAnimal_2;
    [SerializeReference, TypePicker]
    List<IAnimal> animals;

    bool Showw()
    {
        return show1;
    }

    bool Shooow => show2;

    [SerializeReference] Object testObj;
    [SerializeReference, TypePicker] IAnimal testObj2;

    [Serializable]
    class Animal : IAnimal
    {
        [SerializeField] string name;

        public virtual void SaySomething()
        {
            Debug.Log($"My name is {name}");
        }
    }

    void Start()
    {
        foreach (var animal in animals)
            animal.SaySomething();
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
