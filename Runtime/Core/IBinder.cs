using System.Collections.Generic;
using System.Reflection;

namespace Sericaer.UIBind.Runtime.Core
{
    public interface IBinder
    {
        IReadOnlyDictionary<object, string> bindKey2Path { get; }
    }
}