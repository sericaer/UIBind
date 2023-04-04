using Sericaer.UIBind.Runtime;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Sericaer.UIBind.Editor
{
    [CustomEditor(typeof(TextBinder), true)]
    public class TextBinderEditor : UnityEditor.Editor
    {
        SerializedProperty contextBind;

        void OnEnable()
        {
            contextBind = serializedObject.FindProperty("contextBind");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(contextBind);

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}