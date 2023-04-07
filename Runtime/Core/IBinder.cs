using System.Collections.Generic;
using System.Reflection;

namespace Sericaer.UIBind.Runtime.Core
{
    public interface IBinder
    {
        (string property, MethodInfo method)[] propertyPath2Updater { get; set; }
    }
}