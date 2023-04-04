using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Sericaer.UIBind.Runtime.Core
{
    public partial class BindCore : MonoBehaviour
    {
        public const string ObjName = "BindCore";

        public bool isDestroyed => _isDestroyed;

        private Dictionary<string, Item> context2Binds = new Dictionary<string, Item>();
        private bool _isDestroyed = false;

        public static void SetContext(string key, INotifyPropertyChanged target)
        {
            var self = GameObject.Find(BindCore.ObjName)?.GetComponent<BindCore>();
            if (self == null)
            {
                throw new Exception("Can not find BindCore!");
            }

            self.context2Binds[key].target = target;
        }

        public void RemoveBind(TextBinder binder)
        {
            var context = binder.FindContext();
            if (context != null)
            {
                context2Binds[context.Key].RemoveBinder(binder);
            }
        }

        public void AddBind(TextBinder binder)
        {
            var context = binder.FindContext();
            if (context == null)
            {
                throw new Exception($"can not find context, when AddBind {binder}");
            }

            if(context2Binds.ContainsKey(context.Key))
            {
                context2Binds[context.Key].AddBinder(binder);
            }
        }

        public void AddBindContext(BindContext bindContext)
        {
            if(context2Binds.ContainsKey(bindContext.Key))
            {
                if(context2Binds[bindContext.Key].gameObjectID == bindContext.gameObject.GetInstanceID())
                {
                    return;
                }

                throw new Exception($"already have bindContext key {bindContext.Key}");
            }

            var bindGroup = new Item(bindContext);
            context2Binds.Add(bindContext.Key, bindGroup);

            foreach(var binder in bindContext.GetComponentsInChildren<TextBinder>())
            {
                bindGroup.AddBinder(binder);
            }
        }

        public void RemoveBindContext(BindContext bindContext)
        {
            if (!context2Binds.ContainsKey(bindContext.Key))
            {
                throw new Exception($"not have bindContext key {bindContext.Key}");
            }

            foreach (var binder in bindContext.GetComponentsInChildren<TextBinder>())
            {
                context2Binds[bindContext.Key].RemoveBinder(binder);
            }

            context2Binds.Remove(bindContext.Key);
        }

        void OnDestroy()
        {
            _isDestroyed = true;

            foreach (var group in context2Binds.Values)
            {
                group.Dispose();
            }

            context2Binds.Clear();
        }
    }
}
