using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{ 
public abstract class MonoBehaviourWithHandles: MonoBehaviour
{
    public abstract void OnDrawHandles();
    public virtual bool InSelfSpace => true;
}
 

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviourWithHandles), true)]
public class HandleDrawerMonoBehaviourEditor : UnityEditor.Editor
{
    void OnSceneGUI()
    {
        if (target is MonoBehaviour monoBehaviour)
        {
            Undo.RecordObject(monoBehaviour, "HandleChanged");
            
            if(target is MonoBehaviourWithHandles handleable)
            {
                if(handleable.InSelfSpace)
                    EasyHandles.PushMatrix(monoBehaviour.transform.localToWorldMatrix);
                
                handleable.OnDrawHandles();
                
                EasyHandles.ClearSettings();
            }
        }
    }
}
#endif
}