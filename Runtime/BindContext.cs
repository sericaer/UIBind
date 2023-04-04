using Sericaer.UIBind.Runtime.Core;
using System;
using UnityEngine;

namespace Sericaer.UIBind.Runtime
{
    public class BindContext : MonoBehaviour
    {
        [SerializeField]
        private string key;

        private BindCore bindCore;

        public string Key => key;

        void OnEnable()
        {
            if (key == "" || key == null)
            {
                throw new Exception($"Key is empty! {this}");
            }

            bindCore = this.FindOrAddBindCore();
            bindCore.AddBindContext(this);
        }

        void OnDestroy()
        {
            if (!bindCore.isDestroyed)
            {
                bindCore.RemoveBindContext(this);
            }
        }
    }
}
