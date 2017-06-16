using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.UI;
[CanEditMultipleObjects]
[CustomEditor(typeof(CircleMenu))]
public class CircleMenuEditor : ScriptlessEditor
{
    SerializedProperty Elasticity;
    SerializedProperty Speed;
    SerializedProperty IsElastic;
    SerializedProperty NumbersButton;
    SerializedProperty ListButton;
    private CircleMenu MainTarget;
    private bool OldState;
    void OnEnable()
    {
        MainTarget = (CircleMenu)target;
        if (MainTarget != null)
        {
            Elasticity = serializedObject.FindProperty("m_fElasticity");
            Speed = serializedObject.FindProperty("m_fSpeedElastic");
            IsElastic = serializedObject.FindProperty("isElastic");
            NumbersButton = serializedObject.FindProperty("NumbersButton");
            ListButton = serializedObject.FindProperty("LtBtn");
            OldState = IsElastic.boolValue;
            Elasticity.floatValue = OldState ? 1 : 0;
        }
    }
    
    protected override void OnAfterDefaultInspector()
    {        
        base.OnAfterDefaultInspector();


        if (IsElastic.boolValue != OldState)
        {
            OldState = IsElastic.boolValue;
            Elasticity.floatValue = OldState ? 1 : 0;
        }
        if (IsElastic.boolValue)
        {
            GUILayout.Label("Elastic");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(((int)(Elasticity.floatValue * 100)).ToString());
            Elasticity.floatValue = GUILayout.HorizontalSlider(Elasticity.floatValue, 0, 1, GUILayout.Width(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Label("Speed");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(Mathf.RoundToInt(Speed.floatValue).ToString());
            Speed.floatValue = GUILayout.HorizontalSlider(Speed.floatValue, 1, 100, GUILayout.Width(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        NumbersButton.intValue = ListButton.arraySize;
        GUILayout.Label(NumbersButton.intValue + " Child RectTransform in circle");
    }
}
#endif