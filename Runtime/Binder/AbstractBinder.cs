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

        public IReadOnlyDictionary<object, string> bindKey2Path { get; private set; }

        protected virtual void Awake()
        {
            bindKey2Path = bindItems.ToDictionary(p => (object)p.key, p => p.path);
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

            bindContext.OnEnableBinder(this);
        }

        protected virtual void OnDisable()
        {
            bindContext?.OnDisableBinder(this);
        }

        protected virtual void OnDestroy()
        {
            bindContext?.OnDestroyBinder(this);
        }

        protected void UpdateSource(object key, object value)
        {
            bindContext?.UpdateSource(key, value);
        }
    }
}