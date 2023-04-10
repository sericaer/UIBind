using Sericaer.UIBind.Runtime.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace Sericaer.UIBind.Runtime
{
    public class BindContext : MonoBehaviour
    {
        private BindCore core { get; } = new BindCore();

        void OnDestroy()
        {
            core.Dispose();
        }

        void OnEnable()
        {
            core.Enable();
        }

        void OnDisable()
        {
            core.Disable();
        }

        internal void AddBinder(IBinder binder)
        {
            core.OnAddBinder(binder);
        }

        internal void OnDisableBinder(IBinder binder)
        {
            core.OnDisableBinder(binder);
        }

        internal void OnEnableBinder(IBinder binder)
        {
            core.OnEnableBinder(binder);
        }

        internal void OnDestroyBinder(IBinder binder)
        {
            core.OnRemoveBinder(binder);
        }

        internal void SetContextData(INotifyPropertyChanged data)
        {
            if(!isActiveAndEnabled)
            {
                throw new Exception("can not set context data to disabled BindContext");
            }

            core.contextData = data;
        }

        internal void UpdateSource(object key, object value)
        {
            core.UpdateSource(key, value);
        }

    }
}
