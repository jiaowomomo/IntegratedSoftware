using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem.Presenters.Interfaces
{
    public interface ICodeEditor
    {
        TabPage MainCodePage { get; }
        TabPage CurrentCodePage { get; }
        TabPage SelectClosePage { get; }
        TabControl TabControlCode { get; }
        TextBox MessageTextBox { get; }
        ListView ListViewMethod { get; }
        ToolStripStatusLabel CodePathLabel { get; }

        event Action ClearCode;
        event Action OpenCode;
        event Action SaveAsCode;
        event Action SaveAllCode;
        event Action CompileCode;
        event Action StopCode;
        event Action PauseCode;
        event Action StartCode;
        event Action PreviousSearch;
        event Action NextSearch;
        event Action AddSubMethod;
        event Action CloseAllPages;
        event Action SetDefaultHeader;
        event Action SetDefaultSystemReference;
        event Action MoveCustomReference;
        event Action ObtainCustomReference;
        event Action<string> AddCodePage;
        event Action<string> CloseSpecifiedPage;
        event Action<string> CloseOtherPages;
        event Action<string> RemoveSubMethod;
        event Action<string> OpenSubMethod;
        event Action<TabPage> SaveCode;
    }
}
