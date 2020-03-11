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
        [SerializeField]
        RotationSettings rotationSettings = new RotationSettings();
        bool toggleIdentifier,toggleTimeBeforeStopping,toggleReverseRotation,toggleRotationSettings,toggleObjectToRotate,toggleAngleRotation,toggleTimeToRotate;
        bool toggleRotationSettingsChanged,showingMixedValue;
        string identifierValue;
        int timeBeforeStoppingValue = 0;               //time in seconds
        bool reverseRotationValue;
        Transform objectToRotateValue;
        Vector3 angleRotationValue;
        bool foldoutValue;
        Vector2 scrollbarPosition;
        float timeToRotate;                             //time in seconds
        [MenuItem("Window/Custom/Rotators Value Setter")]

        
        public static void ShowWindow()
        {
            GetWindow<RotatorsValueSetter>(false, "Rotators Value Setter", true);
        }
        void OnGUI()
        {
            scrollbarPosition = EditorGUILayout.BeginScrollView(scrollbarPosition);
            rotatorToEdit = GetSelectedRotators(Selection.gameObjects);
            DispalySelection();
            GUILayout.Space(20);
            DisplayElements();
            GUILayout.Space(20);
            Display_SelectedRotators();
            EditorGUILayout.EndScrollView();
            Repaint();
        }
        Rotator[] GetSelectedRotators(GameObject[] gameObjects){
            List<Rotator> selectedRotators = new List<Rotator>();
            foreach(GameObject gameObject in gameObjects)
            {
                Rotator gameObjectRotator = gameObject.GetComponent<Rotator>();
                if(gameObjectRotator != null)
                {
                    selectedRotators.Add(gameObjectRotator);
                }
                SearchRotatorInChild(gameObject.transform,selectedRotators);
            }
            return selectedRotators.ToArray();
        }
        void SearchRotatorInChild(Transform parent,List<Rotator> rotators){
            foreach(Transform child in parent){
                Rotator rotator = child.GetComponent<Rotator>();
                if(rotator != null){
                    rotators.Add(rotator);
                }
                SearchRotatorInChild(child,rotators);

            }
        }
        void DispalySelection()
        {
            
            SerializedObject so = new SerializedObject (this);
            SerializedProperty rotatorToEditProperty = so.FindProperty("rotatorToEdit");
            EditorGUILayout.PropertyField(rotatorToEditProperty, true); // True means show children
            
            
        }
        void DisplayElements()
        {
            EditorGUILayout.LabelField("EDITOR");
            EditorDisplay_Identifier();
            EditorDisplay_TimeBeforeStopping();
            EditorDisplay_ReverseRotation();
            EditorDisplay_RotationsSettings();
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
        void EditorDisplay_RotationsSettings()
        {
            GUILayout.BeginHorizontal();

            if((toggleObjectToRotate || toggleAngleRotation || toggleTimeToRotate) && !(toggleObjectToRotate && toggleAngleRotation && toggleTimeToRotate)){            //if one sub toogle is set to true show mixed value
                bool toggle = toggleRotationSettings;
                EditorGUI.showMixedValue = true;
                toggleRotationSettings = EditorGUILayout.Toggle(toggleRotationSettings,GUILayout.ExpandWidth(false),GUILayout.Width(15));
                toggleRotationSettingsChanged = toggle != toggleRotationSettings ? true : false;
                EditorGUI.showMixedValue = false;
            }
            else if(!toggleRotationSettings && toggleObjectToRotate && toggleAngleRotation && toggleTimeToRotate){                                                      //if all 3 sub toggle are set to true set this to true
                toggleRotationSettings = EditorGUILayout.Toggle(true,GUILayout.ExpandWidth(false),GUILayout.Width(15));
            }   
            else{                                                                                                                                                       //else if non is set to true set it to the value of the toggle
                bool toggle = toggleRotationSettings;
                toggleRotationSettings = EditorGUILayout.Toggle(toggleRotationSettings,GUILayout.ExpandWidth(false),GUILayout.Width(15));
                toggleRotationSettingsChanged = toggle != toggleRotationSettings ? true : false;
                showingMixedValue = false;
            }

            EditorGUIUtility.labelWidth = 220;
            foldoutValue = EditorGUILayout.Foldout(foldoutValue,"Rotations settings");
            GUILayout.EndHorizontal();
            if(foldoutValue){
                EditorGUI.indentLevel++;
                EditorDisplay_ObjectToRotate();
                EditorDisplay_AngleRotation();
                EditorDisplay_TimeToRotate();
                EditorGUI.indentLevel--;
            }
            EditorValidateChanges();
        }
        void EditorDisplay_ObjectToRotate()
        {
            GUILayout.BeginHorizontal();
            if(toggleRotationSettingsChanged){
                toggleObjectToRotate = EditorGUILayout.Toggle(toggleRotationSettings,GUILayout.ExpandWidth(false),GUILayout.Width(25));
            }
            else{
                toggleObjectToRotate = EditorGUILayout.Toggle(toggleObjectToRotate,GUILayout.ExpandWidth(false),GUILayout.Width(25));
            }
            
            EditorGUIUtility.labelWidth = 220;
            objectToRotateValue = EditorGUILayout.ObjectField("Object to Rotate",objectToRotateValue,typeof(Transform),true) as Transform;
            GUILayout.EndHorizontal();
        }
        void EditorDisplay_AngleRotation()
        {
            GUILayout.BeginHorizontal();
            if(toggleRotationSettingsChanged){
                toggleAngleRotation = EditorGUILayout.Toggle(toggleRotationSettings,GUILayout.ExpandWidth(false),GUILayout.Width(25));
            }
            else{
                toggleAngleRotation = EditorGUILayout.Toggle(toggleAngleRotation,GUILayout.ExpandWidth(false),GUILayout.Width(25));
            }
            EditorGUIUtility.labelWidth = 220;
            angleRotationValue = EditorGUILayout.Vector3Field("Angle Rotation",angleRotationValue);
            GUILayout.EndHorizontal();
        }
        void EditorDisplay_TimeToRotate()
        {
            GUILayout.BeginHorizontal();
            if(toggleRotationSettingsChanged){
                toggleTimeToRotate = EditorGUILayout.Toggle(toggleRotationSettings,GUILayout.ExpandWidth(false),GUILayout.Width(25));
            }
            else{
                toggleTimeToRotate = EditorGUILayout.Toggle(toggleTimeToRotate,GUILayout.ExpandWidth(false),GUILayout.Width(25));
            }
            
            EditorGUIUtility.labelWidth = 220;
            timeToRotate = EditorGUILayout.FloatField("Time To Rotate In Seconds",timeToRotate);
            GUILayout.EndHorizontal();
        }
        void EditorValidateChanges()
        {
            if(GUILayout.Button("VALIDATE CHANGES"))
            {
                ApplySettings();
            }
        }
        void Display_SelectedRotators(){
            float numberOfLines = EditorGUIUtility.currentViewWidth / 200;

            foreach(Rotator rotator in rotatorToEdit)
            {
                SerializedObject so = new SerializedObject (rotator);
                SerializedProperty identifierProperty = so.FindProperty("_identifier");
                SerializedProperty timeBeforeStoppingProperty = so.FindProperty("_timeBeforeStoppingInSeconds");
                SerializedProperty reverseRotationProperty = so.FindProperty("_shouldReverseRotation");
                SerializedProperty rotationsSettingsProperty = so.FindProperty("_rotationsSettings");
                EditorGUILayout.PropertyField(identifierProperty, true);
                EditorGUILayout.PropertyField(timeBeforeStoppingProperty, true);
                EditorGUILayout.PropertyField(reverseRotationProperty, true);
                EditorGUILayout.PropertyField(rotationsSettingsProperty, true);
            }
            
        }

        void ApplySettings()
        {
            foreach(Rotator rotator in rotatorToEdit)
            {
                SerializedObject so = new SerializedObject (rotator);
                if(toggleIdentifier)
                {
                    SerializedProperty identifierProperty = so.FindProperty("_identifier");
                    identifierProperty.stringValue = identifierValue;
                }
                if(toggleTimeBeforeStopping)
                {
                    SerializedProperty timeBeforeStoppingProperty = so.FindProperty("_timeBeforeStoppingInSeconds");
                    timeBeforeStoppingProperty.floatValue = timeBeforeStoppingValue;
                }
                if(toggleReverseRotation)
                {
                    SerializedProperty reverseRotationProperty = so.FindProperty("_shouldReverseRotation");
                    reverseRotationProperty.boolValue = reverseRotationValue;
                }
                
                if(!toggleRotationSettings)
                {
                    if(toggleObjectToRotate || toggleAngleRotation ||toggleTimeToRotate)
                    {
                        SerializedProperty rotationsSettingsProperty = so.FindProperty("_rotationsSettings");
                        if(toggleObjectToRotate)
                        {
                        rotationsSettingsProperty.FindPropertyRelative("ObjectToRotate").objectReferenceValue = objectToRotateValue;
                    }
                        if(toggleAngleRotation)
                        {
                            rotationsSettingsProperty.FindPropertyRelative("AngleRotation").vector3Value = angleRotationValue;
                        }
                        if(toggleTimeToRotate)
                        {
                            rotationsSettingsProperty.FindPropertyRelative("TimeToRotateInSeconds").floatValue = timeToRotate;
                        }
                    }
                }
                else 
                {
                    SerializedProperty rotationsSettingsProperty = so.FindProperty("_rotationsSettings");
                    rotationsSettingsProperty.FindPropertyRelative("ObjectToRotate").objectReferenceValue = objectToRotateValue;
                    rotationsSettingsProperty.FindPropertyRelative("AngleRotation").vector3Value = angleRotationValue;
                    rotationsSettingsProperty.FindPropertyRelative("TimeToRotateInSeconds").floatValue = timeToRotate;
                }
                so.ApplyModifiedProperties();
            }
        }
    }


}


