using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{

public class StateShotComponent : MonoBehaviour 
{ 
    public StateShot stateFile;
    bool _recordAtFixedUpdate = false;
    [SerializeField] RecordButton recordState;
    [SerializeField] ApplyButton apply;
    
    public bool HasStateFile => stateFile != null;
    
    public void RecordStateAsync()
    {
        if (Application.isPlaying)
            _recordAtFixedUpdate = true;
        else
            RecordState();
    }

    public void RecordState()
    {
        stateFile.Record(gameObject);
        _recordAtFixedUpdate = false;
        SetDirty(stateFile);
    }

    protected static void SetDirty(ScriptableObject record)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(record);
        AssetDatabase.Refresh();
#endif
    }
    
    void FixedUpdate()
    {
        if (_recordAtFixedUpdate)
            RecordState(); 
    }

    void ApplyState() => stateFile.ApplyToObject(gameObject);
    
        
    [Serializable]
    class RecordButton : InspectorButton<StateShotComponent>
    {
        protected override void OnClick(StateShotComponent obj) => obj.RecordStateAsync();

        protected override bool IsEnabled(StateShotComponent obj) => obj.HasStateFile;
    }
    
            
    [Serializable]
    class ApplyButton : InspectorButton<StateShotComponent>
    {
        protected override void OnClick(StateShotComponent obj) => obj.ApplyState();

        protected override bool IsEnabled(StateShotComponent obj) => obj.HasStateFile;
    }
}
}
