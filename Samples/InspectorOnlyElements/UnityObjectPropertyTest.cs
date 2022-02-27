using System;
using System.Collections.Generic;
using MUtility;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnityObjectPropertyTest : MonoBehaviour
{
    [SerializeField] InspectorMessage message = new InspectorMessage
    {
        messageGetter = parent => 
            "We use UnityObjectProperties ion this component\n" +
            "UnityObjectProperty is a tool to add logic to a UnityObject Reference.\n" +
            "(Use tooltip for more information)",
        messageTypeGetter = parent => InspectorMessageType.Info, 
    };
    
    [Space]
    [Tooltip("This property calls a callback when its changed.")]
    [SerializeField] InspectorUnityObject someMonoBehaviour = new InspectorUnityObject
    {
        type = typeof(MonoBehaviour),
        valueChanged = MonoBehaviourChanged
    }; 
    [Tooltip("This property change it's text & color based on it content.")] 
    [SerializeField] MyCamera someCamera;

    [Tooltip("This property can only be set to a child transform.")] 
    [SerializeField] MyChildTransform someChildTransform;
    
    [Tooltip("This property enabled only, when someCamera property is set. ")]
    [SerializeField] MyTexture someTexture;
 
 
    static void MonoBehaviourChanged(object parent, Object oldValue, Object newValue)
    {
        Debug.Log($"Texture Changed:      {oldValue}   ->   {newValue}");
    }
    
    
    [Serializable]
    class MyCamera : InspectorUnityObject<UnityObjectPropertyTest>
    {
        public override Type ContentType => typeof(Camera);
        protected override Color GetColor(UnityObjectPropertyTest parentObject)
        {
            if(value == null)
                return new Color(1f, 0.56f, 0.52f);
            if(((Camera)value).CompareTag("MainCamera"))
                return new Color(0.76f, 0.89f, 0.49f);
            return Color.white;
        }

        protected override string GetLabel(UnityObjectPropertyTest parentObject, string text)
        {
            if (value == null)
                return text;
            
            var camera = (Camera) value;
            if (!camera.CompareTag("MainCamera"))
                text += " (Not Main Camera)";
            return text;
        }
    }

    [Serializable]
    class MyTexture : InspectorUnityObject<UnityObjectPropertyTest, Texture>
    {
        protected override bool IsEnabled(UnityObjectPropertyTest parentObject) =>
            parentObject.someCamera.Value != null;
    }

    [Serializable]
    class MyChildTransform : InspectorUnityObject<UnityObjectPropertyTest, Transform>
    {
        protected override IList<Transform> PopupElements(UnityObjectPropertyTest parentObject)
        {
            var children = new List<Transform> {null};
            children.AddRange(parentObject.GetComponentsInChildren<Transform>());
            return children;
        }
    }


}
