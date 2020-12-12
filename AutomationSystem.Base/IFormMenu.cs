using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationSystem.Base
{
    public interface IFormMenu
    {
        string MainToolStrip { get; }
        string SubToolStrip { get; }
        void ExecuteTool();
    }
}
