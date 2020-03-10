using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.technical.test
{
    public class RotatorsValueSetter : EditorWindow
    {
        [SerializeField]
        Rotator[] rotatorToEdit;
        bool toggleIdentifier,toggleTimeBeforeStopping,toggleReverseRotation;
        string identifierValue;
        int timeBeforeStoppingValue = 0;
        bool reverseRotationValue;
        [MenuItem("Window/Custom/Rotators Value Setter")]
        
        public static void ShowWindow()
        {
            GetWindow<RotatorsValueSetter>(false, "Rotators Value Setter", true);
        }
        void OnGUI()
        {
            rotatorToEdit = GetSelectedRotators();
            DispalySelection();
            GUILayout.Space(20);
            DisplayElements();
            
            Repaint();
        }
        Rotator[] GetSelectedRotators(){
            List<Rotator> selectedRotators = new List<Rotator>();
            
            foreach(GameObject gameObject in Selection.gameObjects)
            {
                Rotator gameObjectRotator = gameObject.GetComponent<Rotator>();
                if(gameObjectRotator != null)
                {
                    selectedRotators.Add(gameObjectRotator);
                }
            }
            return selectedRotators.ToArray();
        }
        void DispalySelection()
        {
            
            SerializedObject so = new SerializedObject (this);
            SerializedProperty stringsProperty = so.FindProperty("rotatorToEdit");
            EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
            
            
        }
        void DisplayElements()
        {
            EditorGUILayout.LabelField("EDITOR");
            EditorDisplay_Identifier();
            EditorDisplay_TimeBeforeStopping();
            EditorDisplay_ReverseRotation();
            Rotator rotator = Selection.activeGameObject.GetComponent<Rotator>();
            SerializedObject so = new SerializedObject (rotator);
            
            SerializedProperty stringsProperty = so.FindProperty("_identifier");
            EditorGUILayout.LabelField("Identifier");   
            EditorGUIUtility.labelWidth = 45;         
            EditorGUILayout.PropertyField(stringsProperty,GUIContent.none, true);
            so.ApplyModifiedProperties(); // Remember to apply modified properties
        }
        void EditorDisplay_Identifier()
        {
            GUILayout.BeginHorizontal();
            toggleIdentifier = EditorGUILayout.Toggle(toggleIdentifier,GUILayout.ExpandWidth(false),GUILayout.Width(15));
            EditorGUIUtility.labelWidth = 220;
            identifierValue = EditorGUILayout.TextField("Identifier :",identifierValue);
            GUILayout.EndHorizontal();
        }
        void EditorDisplay_TimeBeforeStopping()
        {
            GUILayout.BeginHorizontal();
            toggleTimeBeforeStopping = EditorGUILayout.Toggle(toggleTimeBeforeStopping,GUILayout.ExpandWidth(false),GUILayout.Width(15));
            EditorGUIUtility.labelWidth = 220;
            timeBeforeStoppingValue = EditorGUILayout.IntField("Time Before Stopping in Seconds :",timeBeforeStoppingValue);
            GUILayout.EndHorizontal();
        }
        void EditorDisplay_ReverseRotation()
        {
            GUILayout.BeginHorizontal();
            toggleReverseRotation = EditorGUILayout.Toggle(toggleReverseRotation,GUILayout.ExpandWidth(false),GUILayout.Width(15));
            EditorGUIUtility.labelWidth = 220;
            reverseRotationValue = EditorGUILayout.Toggle("Should Reverse Rotation :",reverseRotationValue);
            GUILayout.EndHorizontal();
        }
    }


}


