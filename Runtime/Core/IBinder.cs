using System.Collections.Generic;

namespace Sericaer.UIBind.Runtime.Core
{
    public interface IBinder
    {
        IEnumerable<string> bindPaths { get; }
        void OnPropertyChanged(string propertyName, object sender);
    }
}