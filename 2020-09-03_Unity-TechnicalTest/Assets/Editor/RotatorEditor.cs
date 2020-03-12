using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.technical.test
{

    [CustomEditor(typeof(Rotator)),CanEditMultipleObjects]
    public class RotatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Open Rotator Editor")){
                EditorWindow.GetWindow(typeof(RotatorsValueSetter),false,"Rotator Value Setter",true);
            }
        }
    }
}
