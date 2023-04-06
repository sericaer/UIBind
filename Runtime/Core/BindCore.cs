using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Sericaer.UIBind.Runtime.Core
{
    public class BindCore : IDisposable
    {
        public INotifyPropertyChanged target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target == value)
                {
                    return;
                }

                if (_target != null)
                {
                    _target.PropertyChanged -= Target_PropertyChanged;
                }

                _target = value;
                if(_target == null)
                {
                    return;
                }

                _target.PropertyChanged += Target_PropertyChanged;

                if (enable)
                {
                    foreach (var binder in binders)
                    {
                        foreach (var path in binder.bindPaths)
                        {
                            Target_PropertyChanged(_target, new PropertyChangedEventArgs(path));
                        }
                    }
                }
            }
        }

        private INotifyPropertyChanged _target;
        private HashSet<IBinder> binders;
        private bool enable;

        internal BindCore()
        {
            binders = new HashSet<IBinder>();
        }

        internal void Disable()
        {
            enable = false;
        }

        internal void Enable()
        {
            enable = true;
        }

        internal void AddBinder(IBinder binder)
        {
            binders.Add(binder);
        }

        internal void RemoveBinder(IBinder binder)
        {
            binders.Remove(binder);
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!enable)
            {
                return;
            }

            foreach (var binder in binders)
            {
                binder.OnPropertyChanged(e.PropertyName, sender);
            }
        }

        public void Dispose()
        {
            if (_target != null)
            {
                _target.PropertyChanged -= Target_PropertyChanged;
            }
        }
    }
}
