using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class TextBinder : MonoBehaviour
    {
        [SerializeField]
        private string contextBind;

        //private BindCore bindCore;

        public string ContextBind => contextBind;

        public Text Text => GetComponent<Text>();
        public BindContext bindContext
        {
            get
            {
                var context = GetComponent<BindContext>();
                if (context!= null)
                {
                    return context;
                }

                context = GetComponentInParent<BindContext>();
                return context;
            }
        }

        void OnEnable()
        {
            if (Text == null)
            {
                throw new Exception($"Cannot find Text Component in {this}");
            }

            if (bindContext == null)
            {
                throw new Exception($"Cannot find BindContext Component in {this} or parent");
            }


            bindContext.binders.Add(this);

            //var binders = bindContext.GetComponentsInChildren<TextBinder>();

            //bindCore = GameObject.Find(BindCore.ObjName)?.GetComponent<BindCore>();
            //if(bindCore != null)
            //{
            //    bindCore.AddBind(this);
            //}
        }



        void OnDisable()
        {
            if (bindContext != null)
            {
                bindContext.binders.Remove(this);
            }
            //if (bindCore != null && !bindCore.isDestroyed)
            //{
            //    bindCore.RemoveBind(this);
            //}
        }


    }
}
