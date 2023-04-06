using Sericaer.UIBind.Runtime.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class TextBinder : MonoBehaviour, IBinder
    {
        [SerializeField]
        private string contextBind;

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

        public IEnumerable<string> bindPaths => new string[] { contextBind };

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

            bindContext.AddBinder(this);
        }

        void OnDisable()
        {
            bindContext?.AddBinder(this);
        }

        public void OnPropertyChanged(string propertyName, object sender)
        {
            if (propertyName == ContextBind)
            {
                PropertyInfo prop = sender.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                Text.text = prop.GetValue(sender)?.ToString();
            }
        }
    }
}
