using System;
using System.Collections.Generic; 
using UnityEngine;

public class TypePickerTester : MonoBehaviour
{  
    [SerializeField] NoesArk ark;
    [SerializeField] Cat cat;
    [SerializeField] Dog dog;  
    [SerializeField] Animal animal;
    [SerializeReference, TypePicker] Animal referencedAnimal;
    [SerializeReference, TypePicker] IAnimal iAnimal;
    [SerializeReference, TypePicker]
    List<IAnimal> animals;
    
 
    [Serializable]
    class Animal : IAnimal
    {  
        [SerializeField] string name;
    }

    interface IAnimal 
    {  
    }

    [Serializable]
    class Dog : Animal
    {
        [SerializeField] bool isGoodBoy = true;

        public void Bark()
        {
            Debug.Log(isGoodBoy ? "Woof" : "I hate You!");
        }
    }

    [Serializable]
    class Cat : Animal
    { 
        [SerializeField] int livesLeft = 9;
        
        public void Meow()
        {
            Debug.Log($"I have {livesLeft} lives left");
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
