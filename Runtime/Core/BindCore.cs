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

        internal void OnAddBinder(IBinder binder)
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

        internal void OnDisableBinder(IBinder binder)
        {
            foreach(var elem in binder2Elements[binder])
            {
                elem.isEnable = false;
            }
        }

        internal void OnEnableBinder(IBinder binder)
        {
            if(!binder2Elements.ContainsKey(binder))
            {
                OnAddBinder(binder);
            }

            foreach (var elem in binder2Elements[binder])
            {
                elem.isEnable = true;
            }
        }

        internal void OnRemoveBinder(IBinder binder)
        {
            binder2Elements.Remove(binder);
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!enable)
            {
                return;
            }

            foreach(var element in binder2Elements.Values.SelectMany(x=>x).Where(x=>x.isEnable))
            {
                if(element.bindPath == e.PropertyName)
                {
                    element.UpdateTarget(sender);
                }
            }
        }

        public void Dispose()
        {
            if (_contextData != null)
            {
                _contextData.PropertyChanged -= Target_PropertyChanged;
            }

            binder2Elements.Clear();
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
