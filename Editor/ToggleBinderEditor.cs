using Sericaer.UIBind.Runtime;
using UnityEditor;

namespace Sericaer.UIBind.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ToggleBinder), true)]
    public class ToggleBinderEditor : ComponentBinderEditor
    {

    }
#endif
}