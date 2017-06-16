using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
public class CustomContext
{
    public static GameObject _ClickedObject = null;
    static CustomContext()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        // Whether this object was right clicked
        if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
            && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
        {
            // Find what object this is
            GameObject clickedObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (clickedObject)
            {
                _ClickedObject = clickedObject;                    
                // Consume the event to remove Unity's default context menu
                // Event.current.Use();
            }
        }    
    }
}

public class ConfigEditor
{

    public static void NewTag(string _tag)
    {
        SerializedObject tagMag = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagMag.FindProperty("tags");
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(_tag))
            {
                found = true; 
                break;
            }
        }
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = _tag;
            tagMag.ApplyModifiedProperties();
        }
    }
    public static GameObject CreateCanvas()
    {
        if (GameObject.FindObjectOfType(typeof(EventSystem)) == null)
        {
            GameObject evnt = new GameObject("EventSystem", typeof(EventSystem));
            evnt.AddComponent(typeof(StandaloneInputModule));
        }
        GameObject go = new GameObject("Canvas", typeof(Canvas));
        go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        go.AddComponent(typeof(CanvasScaler));
        go.AddComponent(typeof(GraphicRaycaster));
        return go;
    }
}
#endif