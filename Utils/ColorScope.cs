using System;
using UnityEngine;

namespace Sericaer.UIBind.Runtime
{
    struct ColorScope : IDisposable
    {
        public Color OriginalColor;

        public ColorScope(Color color, Func<bool> predicate = null)
        {
            OriginalColor = GUI.color;
            if (predicate == null || predicate())
            {
                GUI.color = color;
            }
        }

        public void Dispose()
        {
            GUI.color = OriginalColor;
        }
    }
}