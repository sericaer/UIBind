using Sericaer.UIBind.Runtime;
using UnityEditor;

namespace Sericaer.UIBind.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(TextBinder), true)]
    public class TextBinderEditor : ComponentBinderEditor
    {

    }
#endif
}