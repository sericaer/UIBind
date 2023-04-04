using Sericaer.UIBind.Runtime.Core;
using UnityEngine;

namespace Sericaer.UIBind.Runtime
{
    static class UIBindExtentions
    {
        public static BindContext FindContext(this MonoBehaviour binder)
        {
            var context = binder.GetComponent<BindContext>();
            if (context == null)
            {
                context = binder.GetComponentInParent<BindContext>();
            }

            return context;
        }

        public static BindCore FindOrAddBindCore(this MonoBehaviour binder)
        {
            var bindMgr = GameObject.Find(BindCore.ObjName);
            if (bindMgr == null)
            {
                bindMgr = new GameObject();
                bindMgr.name = BindCore.ObjName;

                bindMgr.AddComponent<BindCore>();
            }

            return bindMgr.GetComponent<BindCore>();
        }
    }
}