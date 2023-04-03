using Sericaer.UIBind.Runtime;
using System;
using UnityEditor;
using UnityEngine;

namespace Sericaer.UIBinds.Editor
{
    [CustomEditor(typeof(BindContext), true)]
    public class BindContextEditor : UnityEditor.Editor
    {
        SerializedProperty Key;

        void OnEnable()
        {
            Key = serializedObject.FindProperty("_key");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            using (new ColorScope(Color.red, () => string.IsNullOrEmpty(Key.stringValue)))
            {
                EditorGUILayout.PropertyField(Key);
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}