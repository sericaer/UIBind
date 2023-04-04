using Sericaer.UIBind.Runtime.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class TextBinder : MonoBehaviour
    {
        [SerializeField]
        private string contextBind;

        private BindCore bindCore;

        public string ContextBind => contextBind;
        public Text Text => GetComponent<Text>();

        void OnEnable()
        {
            if(Text == null)
            {
                throw new Exception($"Cannot find Text Component in {this}");
            }

            bindCore = GameObject.Find(BindCore.ObjName)?.GetComponent<BindCore>();
            if(bindCore != null)
            {
                bindCore.AddBind(this);
            }
        }

        void OnDisable()
        {
            if (bindCore != null && !bindCore.isDestroyed)
            {
                bindCore.RemoveBind(this);
            }
        }
    }
}
