using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.technical.test
{
    [CustomPropertyDrawer(typeof(RotationSettings))]
    public class RotationSettingPropertyDrawer : PropertyDrawer
    {
        public Dictionary<string, bool> unfold = new Dictionary<string, bool>();
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 16;

            if(!unfold.ContainsKey(property.propertyPath))
            {
                unfold.Add(property.propertyPath,true);
            }
            unfold[property.propertyPath] = EditorGUI.Foldout (position, unfold[property.propertyPath], label);

            if(unfold[property.propertyPath])                                                                           //Display sub properties
            {
                EditorGUIUtility.labelWidth = 130;
                EditorGUI.indentLevel++;
                EditorGUILayout.ObjectField("Object To Rotate",property.FindPropertyRelative("ObjectToRotate").objectReferenceValue,typeof(Transform),true,GUILayout.Width(350),GUILayout.ExpandWidth(false));
                GUILayout.BeginHorizontal(GUILayout.Width(250),GUILayout.ExpandWidth(false));
                EditorGUIUtility.labelWidth = 1;
                EditorGUILayout.LabelField("Angle Rotation",GUILayout.Width(120),GUILayout.ExpandWidth(false));
                EditorGUILayout.Vector3Field(GUIContent.none,property.FindPropertyRelative("AngleRotation").vector3Value,GUILayout.Width(150),GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = 220;
                EditorGUILayout.FloatField("Time To Rotate In Seconds",property.FindPropertyRelative("TimeToRotateInSeconds").floatValue,GUILayout.Width(250),GUILayout.ExpandWidth(false));
                EditorGUI.indentLevel--;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (unfold.ContainsKey (property.propertyPath))
            {
                return unfold [property.propertyPath] ? 20 : 16;
            }
            else
            {
                return 16;
            }
        }
    }
}
