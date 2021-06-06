using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public interface IFormMenu
    {
        string MainToolStrip { get; }
        string SubToolStrip { get; }
        void ExecuteTool();
    }
}
