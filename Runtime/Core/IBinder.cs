using System.Collections.Generic;
using System.Reflection;

namespace Sericaer.UIBind.Runtime.Core
{
    public interface IBinder
    {
        (string property, MethodInfo method)[] property2Method { get; set; }
    }
}