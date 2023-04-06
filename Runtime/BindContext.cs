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
        private BindCore core;

        void Awake()
        {
            core = new BindCore();
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
            core.AddBinder(binder);
        }

        internal void RemoveBinder(IBinder binder)
        {
            core.RemoveBinder(binder);
        }

        internal void SetTarget(INotifyPropertyChanged target)
        {
            core.target = target;
        }
    }
}
