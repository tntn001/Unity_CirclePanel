using UnityEngine;
using UnityEditor;

public abstract class ScriptlessEditor : Editor
{
    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        OnBeforeDefaultInspector();
        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
        OnAfterDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void OnBeforeDefaultInspector()
    {
        
    }

    protected virtual void OnAfterDefaultInspector()
    {
        
    }
   


}
