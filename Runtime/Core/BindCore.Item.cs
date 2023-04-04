using Sericaer.UIBind.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Sericaer.UIBind.Runtime.Core
{
    public partial class BindCore
    {
        private class Item : IDisposable
        {
            public readonly string Key;
            public readonly int gameObjectID;

            private INotifyPropertyChanged _target;
            private HashSet<TextBinder> binders;

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
                    if (_target != null)
                    {
                        _target.PropertyChanged += Target_PropertyChanged;

                        foreach(var binder in binders)
                        {
                            Target_PropertyChanged(_target, new PropertyChangedEventArgs(binder.ContextBind));
                        }
                    }
                }
            }

            public Item(BindContext bindContext)
            {
                this.Key = bindContext.Key;
                this.gameObjectID = bindContext.gameObject.GetInstanceID();

                this.binders = new HashSet<TextBinder>();
            }

            public void Dispose()
            {
                if (_target != null)
                {
                    _target.PropertyChanged -= Target_PropertyChanged;
                }
            }

            private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                foreach (var binder in binders)
                {
                    if (e.PropertyName == binder.ContextBind)
                    {
                        PropertyInfo prop = sender.GetType().GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                        binder.Text.text = prop.GetValue(sender)?.ToString();
                    }
                }
            }

            internal void RemoveBinder(TextBinder textBind)
            {
                binders.Remove(textBind);
            }

            internal void AddBinder(TextBinder textBind)
            {
                binders.Add(textBind);
            }
        }
    }
}