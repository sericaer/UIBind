using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace Sericaer.UIBind.Runtime.Core
{
    public class BindCore : IDisposable
    {
        public INotifyPropertyChanged contextData
        {
            get
            {
                return _contextData;
            }
            set
            {
                if (_contextData == value)
                {
                    return;
                }

                if (_contextData != null)
                {
                    _contextData.PropertyChanged -= Target_PropertyChanged;
                }

                _contextData = value;
                if(_contextData == null)
                {
                    return;
                }

                _contextData.PropertyChanged += Target_PropertyChanged;

                if (enable)
                {
                    foreach (var binder in binders)
                    {
                        foreach (var pair in binder.propertyPath2Updater)
                        {
                            Target_PropertyChanged(_contextData, new PropertyChangedEventArgs(pair.Item1));
                        }
                    }
                }
            }
        }

        private INotifyPropertyChanged _contextData;
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
            if (binders.Add(binder) && _contextData != null)
            {
                foreach (var pair in binder.propertyPath2Updater)
                {
                    Target_PropertyChanged(_contextData, new PropertyChangedEventArgs(pair.Item1));
                }
            }
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
                foreach(var pair in binder.propertyPath2Updater)
                {
                    if(pair.property == e.PropertyName)
                    {
                        var prop = sender.GetType().GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                        var parameters = pair.method.GetParameters();
                        if(parameters.Length != 1)
                        {
                            throw new Exception();
                        }

                        if(parameters[0].ParameterType == prop.PropertyType)
                        {
                            pair.method.Invoke(binder, new object[] { prop.GetValue(sender) });
                        }
                        else if(parameters[0].ParameterType == typeof(string))
                        {
                            pair.method.Invoke(binder, new object[] { prop.GetValue(sender).ToString() });
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_contextData != null)
            {
                _contextData.PropertyChanged -= Target_PropertyChanged;
            }
        }

        internal Action<object> GetSetter(string path)
        {
            PropertyInfo property = contextData.GetType().GetProperty(path);

            return (object value) =>
            {
                property.GetSetMethod().Invoke(contextData, new object[] { value });
            };
        }
    }
}
