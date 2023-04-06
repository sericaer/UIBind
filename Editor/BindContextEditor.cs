using Sericaer.UIBind.Runtime;
using System;
using UnityEditor;
using UnityEngine;

namespace Sericaer.UIBind.Editor
{
    [CustomEditor(typeof(BindContext), true)]
    public class BindContextEditor : UnityEditor.Editor
    {
        //SerializedProperty Key;

        //void OnEnable()
        //{
        //    Key = serializedObject.FindProperty("key");
        //}

        //public override void OnInspectorGUI()
        //{
        //    this.serializedObject.Update();

        //    EditorGUILayout.PropertyField(Key);

        //    this.serializedObject.ApplyModifiedProperties();
        //}
    }
}