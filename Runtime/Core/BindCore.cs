using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

using UnityEngine;

namespace Sericaer.UIBind.Runtime.Core
{
    public partial class BindCore : IDisposable
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

                foreach (var elem in binder2Elements.Values.SelectMany(x => x))
                {
                    elem.DisassocSource(_contextData);
                }

                _contextData = value;
                if(_contextData == null)
                {
                    return;
                }

                _contextData.PropertyChanged += Target_PropertyChanged;
                foreach(var elem in binder2Elements.Values.SelectMany(x => x))
                {
                    elem.AssocSource(_contextData);
                }

                if (enable)
                {
                    foreach (var bindPath in binder2Elements.Values.SelectMany(x => x).Select(x => x.bindPath).Distinct())
                    {
                        Target_PropertyChanged(_contextData, new PropertyChangedEventArgs(bindPath));
                    }
                }
            }
        }

        private bool enable;

        private INotifyPropertyChanged _contextData;

        private Dictionary<IBinder, Element[]> binder2Elements;

        internal BindCore()
        {
            binder2Elements = new Dictionary<IBinder, Element[]>();
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
            binder2Elements.Add(binder, binder.bindKey2Path.Select(x => new Element(binder, x.Key, x.Value)).ToArray());

            if (_contextData != null)
            {
                foreach (var element in binder2Elements.Values.SelectMany(x=>x))
                {
                    element.AssocSource(_contextData);
                }

                foreach (var bindPath in binder2Elements.Values.SelectMany(x => x).Select(x=>x.bindPath).Distinct())
                {
                    Target_PropertyChanged(_contextData, new PropertyChangedEventArgs(bindPath));
                }
            }
        }

        internal void RemoveBinder(IBinder binder)
        {
            binder2Elements.Remove(binder);
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!enable)
            {
                return;
            }

            foreach(var element in binder2Elements.Values.SelectMany(x=>x))
            {
                if(element.bindPath == e.PropertyName)
                {
                    element.UpdateTarget(sender);
                }
            }

            //foreach (var binder in binders)
            //{
            //    foreach(var pair in binder.propertyPath2Updater)
            //    {
            //        if(pair.property == e.PropertyName)
            //        {
            //            var prop = sender.GetType().GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            //            var parameters = pair.method.GetParameters();
            //            if(parameters.Length != 1)
            //            {
            //                throw new Exception();
            //            }

            //            if(parameters[0].ParameterType == prop.PropertyType)
            //            {
            //                pair.method.Invoke(binder, new object[] { prop.GetValue(sender) });
            //            }
            //            else if(parameters[0].ParameterType == typeof(string))
            //            {
            //                pair.method.Invoke(binder, new object[] { prop.GetValue(sender).ToString() });
            //            }
            //            else
            //            {
            //                throw new Exception();
            //            }
            //        }
            //    }
            //}
        }

        public void Dispose()
        {
            if (_contextData != null)
            {
                _contextData.PropertyChanged -= Target_PropertyChanged;
            }
        }

        public void UpdateSource(object key, object value)
        {
            foreach(var elem in binder2Elements.Values.SelectMany(list => list.Where(elem=> elem.key.Equals(key))))
            {
                elem.UpdateSource(value);
            }
        }
    }
}
