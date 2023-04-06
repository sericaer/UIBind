using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace Sericaer.UIBind.Runtime
{
    public class BindContext : MonoBehaviour
    {
        //[SerializeField]
        //private string key;

        //private BindCore bindCore;

        //public string Key => key;


        public HashSet<TextBinder> binders
        {
            get
            {
                if(_binders == null)
                {
                    _binders = new HashSet<TextBinder>();
                }

                return _binders;
            }
        }

        public INotifyPropertyChanged target
        {
            get
            {
                return _target;
            }
            set
            {
                if(_target == value)
                {
                    return;
                }

                if(_target != null)
                {
                    _target.PropertyChanged -= _target_PropertyChanged;
                }

                _target = value;

                if(this.enabled)
                {
                    _target.PropertyChanged += _target_PropertyChanged;
                }
            }
        }

        private INotifyPropertyChanged _target;
        private HashSet<TextBinder> _binders;

        void OnEnable()
        {
            if(_target != null)
            {
                _target.PropertyChanged += _target_PropertyChanged;
            }


            //if (key == "" || key == null)
            //{
            //    throw new Exception($"Key is empty! {this}");
            //}

            //bindCore = this.FindOrAddBindCore();
            //bindCore.AddBindContext(this);
        }

        void OnDisable()
        {
            if (_target != null)
            {
                _target.PropertyChanged -= _target_PropertyChanged;
            }

            //if (!bindCore.isDestroyed)
            //{
            //    bindCore.RemoveBindContext(this);
            //}
        }

        private void _target_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
    }
}
