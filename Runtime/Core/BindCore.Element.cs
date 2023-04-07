using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

using UnityEngine;

namespace Sericaer.UIBind.Runtime.Core
{
    public partial class BindCore
    {
        class Element
        {
            public readonly object key;
            public readonly string bindPath;

            private readonly IBinder target;
            private INotifyPropertyChanged source;

            private readonly MethodInfo updateTargetMethod;

            private PropertyInfo sourceProperty;

            public Element(IBinder target, object key, string bindPath)
            {
                this.key = key;
                this.bindPath = bindPath;
                this.target = target;

                updateTargetMethod = target.GetType().GetMethods(BindingFlags.Public
                                                         | BindingFlags.NonPublic
                                                         | BindingFlags.Instance
                                                         | BindingFlags.DeclaredOnly)
                    .SingleOrDefault(method =>
                    {
                        var attrib = method.GetCustomAttribute<PropertyChangedAttribute>();
                        return attrib != null && attrib.propertyEnum.Equals(key);
                    });
            }

            public void UpdateTarget(object currValue)
            {
                updateTargetMethod.Invoke(target, new object[] { sourceProperty.GetValue(currValue) });
            }

            public void UpdateSource(object currValue)
            {
                sourceProperty.SetValue(source, currValue);
            }

            public void AssocSource(INotifyPropertyChanged source)
            {
                this.source = source;
                sourceProperty = source.GetType().GetProperty(bindPath);
            }

            public void DisassocSource(INotifyPropertyChanged source)
            {
                if(this.source != source)
                {
                    throw new Exception();
                }

                this.source = null;
                sourceProperty = null;
            }
        }
    }
}
