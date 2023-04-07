using Sericaer.UIBind.Runtime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Sericaer.UIBind.Runtime
{
    public abstract class AbstractBinder<TPropertyEnum, TTarget> : MonoBehaviour, IBinder
        where TPropertyEnum : Enum
    {
        [Serializable]
        public class BindItem
        {
            public TPropertyEnum key;
            public string path;
        }

        [SerializeField]
        public BindItem[] bindItems;

        public (string, MethodInfo)[] propertyPath2Updater { get; set; }

        protected Dictionary<object, MethodInfo> propertyKey2Setter { get; set; }

        protected TTarget target => GetComponent<TTarget>();

        protected BindContext bindContext
        {
            get
            {
                var context = GetComponent<BindContext>();
                if (context != null)
                {
                    return context;
                }

                context = GetComponentInParent<BindContext>();
                return context;
            }
        }

        protected virtual void Awake()
        {
            var dictUpdater = new Dictionary<object, MethodInfo>();
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public
                                                     | BindingFlags.NonPublic
                                                     | BindingFlags.Instance
                                                     | BindingFlags.DeclaredOnly))
            {
                var attrib = method.GetCustomAttribute<PropertyChangedAttribute>();
                if(attrib == null)
                {
                    continue;
                }

                dictUpdater.Add(attrib.propertyEnum, method);
            }

            propertyPath2Updater = bindItems.Select(item =>(item.path, dictUpdater[item.key])).ToArray();
        }

        protected virtual void OnEnable()
        {
            if (target == null)
            {
                throw new Exception($"Cannot find target Component in {this}");
            }

            if (bindContext == null)
            {
                throw new Exception($"Cannot find BindContext Component in {this} or parent");
            }

            bindContext.AddBinder(this);
        }

        protected virtual void OnDisable()
        {
            bindContext?.RemoveBinder(this);
        }

        protected void UpdateSource(object key, object value)
        {
            var item = bindItems.SingleOrDefault(x => x.key.Equals(key));
            var setter = bindContext?.GetSetter(item.path);

            setter?.Invoke(value);
        }
    }
}