using UnityEditor;

namespace Sericaer.UIBind.Editor
{
    public class ComponentBinderEditor : UnityEditor.Editor
    {
        SerializedProperty bindItems;

        void OnEnable()
        {
            bindItems = serializedObject.FindProperty("bindItems");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(bindItems);

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}