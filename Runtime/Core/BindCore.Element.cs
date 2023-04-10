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
            public readonly BindWay bindWay;
            
            private readonly IBinder target;
            private INotifyPropertyChanged source;

            private readonly MethodInfo updateTargetMethod;

            private MethodInfo sourceGetter;
            private MethodInfo sourceSetter;

            private Converter converter;

            public Element(IBinder target, object key, string bindPath)
            {
                this.key = key;
                this.bindPath = bindPath;
                this.target = target;

                var enumType = key.GetType();
                var memberInfos = enumType.GetMember(key.ToString());
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var bindWayAttribute = (BindWayAttribute)enumValueMemberInfo.GetCustomAttribute(typeof(BindWayAttribute), false);
                
                bindWay = bindWayAttribute.bindWay;
                updateTargetMethod = target.GetType().GetMethod(bindWayAttribute.updateTargetMethod, BindingFlags.Public
                                                         | BindingFlags.NonPublic
                                                         | BindingFlags.Instance
                                                         | BindingFlags.DeclaredOnly);
            }

            public void UpdateTarget(object currValue)
            {
                updateTargetMethod.Invoke(target, new object[] { converter.ConvertSourceToTargetValue(sourceGetter.Invoke(currValue, null)) });
            }

            public void UpdateSource(object currValue)
            {
                sourceSetter.Invoke(source, new object[] { currValue });
            }

            public void AssocSource(INotifyPropertyChanged source)
            {
                this.source = source;

                var property = source.GetType().GetProperty(bindPath);
                if(property == null)
                {
                    throw new Exception($"Can not find bindPath '{bindPath}' in {source.GetType()}");
                }

                converter = new Converter(property.PropertyType, updateTargetMethod.GetParameters().First().ParameterType);

                sourceGetter = property.GetGetMethod();
                sourceSetter = property.GetSetMethod();

                if(bindWay == BindWay.TwoWay && sourceSetter == null)
                {
                    throw new Exception();
                }
            }

            public void DisassocSource(INotifyPropertyChanged source)
            {
                if(this.source != source)
                {
                    throw new Exception();
                }

                this.source = null;

                sourceGetter = null;
                sourceSetter = null;

                converter = null;
            }
        }
    }

    public class Converter
    {
        private readonly Type targetValueType;
        private readonly Type sourceType;

        public Converter(Type sourceType, Type targetValueType)
        {
            this.sourceType = sourceType;
            this.targetValueType = targetValueType;
        }

        internal object ConvertSourceToTargetValue(object sourceValue)
        {
            if(sourceValue == null)
            {
                return null;
            }
            else if(sourceValue.GetType() == targetValueType)
            {
                return sourceValue;
            }
            else if(targetValueType.IsAssignableFrom(sourceValue.GetType()))
            {
                return sourceValue;
            }
            else if (targetValueType == typeof(string))
            {
                return sourceValue.ToString();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
