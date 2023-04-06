//using Sericaer.UIBind.Runtime.Core;
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
    }
}