using CommonLibrary.CodeSystem.Controls;
using CommonLibrary.CodeSystem.Presenters.Interfaces;
using Microsoft.CSharp;
using ScintillaNET;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem.Views
{
    public partial class CodeEditor : Form, ICodeEditor
    {
        private bool m_bIsSearchShow = false;
        private Point m_searchPanelLocation = Point.Empty;

        private TabPage m_currentCodePage = null;
        private TabPage m_mainCodePage = null;
        private TabPage m_selectClosePage = null;

        public TabPage CurrentCodePage => m_currentCodePage;
        public TabPage MainCodePage => m_mainCodePage;
        public TabPage SelectClosePage => m_selectClosePage;
        public TabControl TabControlCode => tabControlCode;
        public TextBox MessageTextBox => textBoxError;
        public ListView ListViewMethod => listViewMethods;
        public ToolStripStatusLabel CodePathLabel => toolStripStatusLabel1;

        public event Action ClearCode;
        public event Action OpenCode;
        public event Action SaveAsCode;
        public event Action SaveAllCode;
        public event Action CompileCode;
        public event Action StopCode;
        public event Action PauseCode;
        public event Action StartCode;
        public event Action PreviousSearch;
        public event Action NextSearch;
        public event Action AddSubMethod;
        public event Action CloseAllPages;
        public event Action SetDefaultHeader;
        public event Action SetDefaultSystemReference;
        public event Action MoveCustomReference;
        public event Action ObtainCustomReference;
        public event Action<string> AddCodePage;
        public event Action<string> CloseSpecifiedPage;
        public event Action<string> CloseOtherPages;
        public event Action<string> RemoveSubMethod;
        public event Action<string> OpenSubMethod;
        public event Action<TabPage> SaveCode;

        public CodeEditor()
        {
            InitializeComponent();
        }

        private void CodeEdit_Load(object sender, EventArgs e)
        {
            AddCodePage?.Invoke(CodeManager.MAIN_PAGE);
            if (tabControlCode.TabPages.Count > 0)
            {
                m_currentCodePage = tabControlCode.TabPages[0];
                m_mainCodePage = tabControlCode.TabPages[0];
                toolStripStatusLabel1.Text = $"当前脚本: {CodeManager.Instance.MainCodeFullPath}";
            }

            CodeManager.Instance.OnStatusChange += OnStatusChange;
            SetToolStrip();
            InitHotkeys();
            InitSubToolStrip();
            InitSubMethodList();
        }

        private void SetToolStrip()
        {
            switch (CodeManager.Instance.Status)
            {
                case CodeStatus.Idle:
                case CodeStatus.AbnormalStop:
                    toolStripButtonRun.Enabled = true;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = false;
                    toolStripMenuItemRun.Enabled = true;
                    toolStripMenuItemPause.Enabled = false;
                    toolStripMenuItemStop.Enabled = false;
                    GC.Collect();
                    break;
                case CodeStatus.Run:
                    toolStripButtonRun.Enabled = false;
                    toolStripButtonPause.Enabled = true;
                    toolStripButtonStop.Enabled = true;
                    toolStripMenuItemRun.Enabled = false;
                    toolStripMenuItemPause.Enabled = true;
                    toolStripMenuItemStop.Enabled = true;
                    break;
                case CodeStatus.Pause:
                    toolStripButtonRun.Enabled = true;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = true;
                    toolStripMenuItemRun.Enabled = true;
                    toolStripMenuItemPause.Enabled = false;
                    toolStripMenuItemStop.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void InitHotkeys()
        {
            //注册热键
            HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
        }

        private void InitSubToolStrip()
        {
            toolStripButtonDefaultHeader.Visible = false;
            toolStripButtonDefaultSystemReference.Visible = false;
        }

        private void InitSubMethodList()
        {
            foreach (string subMethod in CodeManager.Instance.SubMethods)
            {
                listViewMethods.Items.Add(subMethod, subMethod, null);
            }
        }

        private void InvokeIfNeeded(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        private void toolStripButtonCompile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void toolStripButtonRun_Click(object sender, EventArgs e)
        {
            RunCode();
        }

        private void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            PauseRunningCode();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            StopRunningCode();
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            ClearCurrentPageCode();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenMainCode();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveCurrentPageCode();
        }

        private void OnStatusChange()
        {
            Action action = new Action(() =>
            {
                SetToolStrip();
            });
            InvokeIfNeeded(action);
        }

        private void CodeEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            CodeManager.Instance.OnStatusChange -= OnStatusChange;
        }

        private void tabControlCode_Selected(object sender, TabControlEventArgs e)
        {
            m_currentCodePage = e.TabPage;
            if (m_bIsSearchShow)
            {
                CodeEditPage codeEditPage = null;
                if (CurrentCodePage != null)
                {
                    codeEditPage = CurrentCodePage.Controls[0] as CodeEditPage;
                }
                SearchManager.TextArea = codeEditPage;
                SearchManager.Find(true);
            }
            toolStripButtonDefaultHeader.Visible = m_currentCodePage.Name.Equals(CodeManager.HEADER_PAGE, StringComparison.OrdinalIgnoreCase);
            toolStripButtonDefaultSystemReference.Visible = m_currentCodePage.Name.Equals(CodeManager.SYSTEM_REFERENCE_PAGE, StringComparison.OrdinalIgnoreCase);
        }

        #region Search

        private void OpenSearch()
        {
            SearchManager.SearchBox = TxtSearch;
            CodeEditPage codeEditPage = null;
            if (CurrentCodePage != null)
            {
                codeEditPage = CurrentCodePage.Controls[0] as CodeEditPage;
            }
            SearchManager.TextArea = codeEditPage;

            if (!m_bIsSearchShow)
            {
                m_bIsSearchShow = true;
                InvokeIfNeeded(delegate ()
                {
                    PanelSearch.Visible = true;
                    TxtSearch.Text = SearchManager.LastSearch;
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
            else
            {
                InvokeIfNeeded(delegate ()
                {
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchManager.Find(true);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyManager.IsHotkey(e, Keys.Enter))
            {
                SearchManager.Find(true);
            }
            if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true))
            {
                SearchManager.Find(false);
            }
        }

        private void BtnPrevSearch_Click(object sender, EventArgs e)
        {
            PreviousSearch?.Invoke();
        }

        private void BtnNextSearch_Click(object sender, EventArgs e)
        {
            NextSearch?.Invoke();
        }

        private void BtnCloseSearch_Click(object sender, EventArgs e)
        {
            if (m_bIsSearchShow)
            {
                m_bIsSearchShow = false;
                SearchManager.TextArea = null;
                SearchManager.SearchBox = null;
                InvokeIfNeeded(delegate ()
                {
                    PanelSearch.Visible = false;
                });
            }
        }

        private void PanelSearch_MouseDown(object sender, MouseEventArgs e)
        {
            m_searchPanelLocation = Cursor.Position;
            PanelSearch.Cursor = Cursors.NoMove2D;
        }

        private void PanelSearch_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int nOffsetX = Cursor.Position.X - m_searchPanelLocation.X;
                int nOffsetY = Cursor.Position.Y - m_searchPanelLocation.Y;
                PanelSearch.Location = new Point(PanelSearch.Location.X + nOffsetX, PanelSearch.Location.Y + nOffsetY);

                m_searchPanelLocation = Cursor.Position;
            }
        }

        private void PanelSearch_MouseUp(object sender, MouseEventArgs e)
        {
            PanelSearch.Cursor = Cursors.Default;
        }

        #endregion

        private void toolStripButtonAddSub_Click(object sender, EventArgs e)
        {
            SetSubMethod();
        }

        private void toolStripButtonRemoveSub_Click(object sender, EventArgs e)
        {
            RemoveSub();
        }

        private void toolStripButtonOpenSub_Click(object sender, EventArgs e)
        {
            OpenSub();
        }

        private void toolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            CloseSpecifiedEditPage(m_selectClosePage);
        }

        private void toolStripMenuItemCloseOther_Click(object sender, EventArgs e)
        {
            CloseOtherEditPages(m_selectClosePage);
        }

        private void toolStripMenuItemCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllEditPages();
        }

        private void toolStripButtonSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllPagesCode();
        }

        private void toolStripButtonOpenHeader_Click(object sender, EventArgs e)
        {
            OpenHeaderCode();
        }

        private void toolStripButtonDefaultHeader_Click(object sender, EventArgs e)
        {
            SetDefaultHeaderPage();
        }

        private void toolStripButtonOpenSystemReference_Click(object sender, EventArgs e)
        {
            OpenSystemReferenceCode();
        }

        private void toolStripButtonDefaultSystemReference_Click(object sender, EventArgs e)
        {
            SetDefaultSystemReferencePage();
        }

        private void toolStripButtonOpenCustomReference_Click(object sender, EventArgs e)
        {
            OpenCustomReferenceCode();
        }

        private void toolStripButtonMoveCustomReference_Click(object sender, EventArgs e)
        {
            MoveCustomReferenceLibrary();
        }

        private void toolStripButtonObtainCustomReference_Click(object sender, EventArgs e)
        {
            ObtainCustomReferenceLibrary();
        }

        private void toolStripButtonPreview_Click(object sender, EventArgs e)
        {
            Preview();
        }

        private void listViewMethods_DoubleClick(object sender, EventArgs e)
        {
            OpenSubMethodPage();
        }

        private void toolStripMenuItemCreateSub_Click(object sender, EventArgs e)
        {
            SetSubMethod();
        }

        private void toolStripMenuItemRemoveSub_Click(object sender, EventArgs e)
        {
            RemoveSub();
        }

        private void toolStripMenuItemOpenSub_Click(object sender, EventArgs e)
        {
            OpenSubMethodPage();
        }

        private void toolStripMenuItemCloseSub_Click(object sender, EventArgs e)
        {
            if (listViewMethods.FocusedItem != null)
            {
                string strMethodName = listViewMethods.FocusedItem.Text;
                CloseSpecifiedPage?.Invoke(strMethodName);
            }
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsMainCode();
        }

        private void tabControlCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_selectClosePage = GetTabPageByLocation(e.Location);
            }
        }

        private TabPage GetTabPageByLocation(Point point)
        {
            for (int i = 0; i < tabControlCode.TabPages.Count; i++)
            {
                if (tabControlCode.GetTabRect(i).Contains(point))
                {
                    return tabControlCode.TabPages[i];
                }
            }
            return null;
        }

        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            OpenMainCode();
        }

        private void toolStripMenuItemOpenHeader_Click(object sender, EventArgs e)
        {
            OpenHeaderCode();
        }

        private void toolStripMenuItemOpenSystemReference_Click(object sender, EventArgs e)
        {
            OpenSystemReferenceCode();
        }

        private void toolStripMenuItemOpenCustomReference_Click(object sender, EventArgs e)
        {
            OpenCustomReferenceCode();
        }

        private void toolStripMenuItemAddSub_Click(object sender, EventArgs e)
        {
            SetSubMethod();
        }

        private void toolStripMenuItemMoveCustomReference_Click(object sender, EventArgs e)
        {
            MoveCustomReferenceLibrary();
        }

        private void toolStripMenuItemObtainCustomReference_Click(object sender, EventArgs e)
        {
            ObtainCustomReferenceLibrary();
        }

        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsMainCode();
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SaveCurrentPageCode();
        }

        private void toolStripMenuItemSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllPagesCode();
        }

        private void toolStripMenuItemCloseThisPage_Click(object sender, EventArgs e)
        {
            CloseSpecifiedEditPage(m_currentCodePage);
        }

        private void toolStripMenuItemCloseOtherPages_Click(object sender, EventArgs e)
        {
            CloseOtherEditPages(m_currentCodePage);
        }

        private void toolStripMenuItemCloseAllPages_Click(object sender, EventArgs e)
        {
            CloseAllEditPages();
        }

        private void toolStripMenuItemFind_Click(object sender, EventArgs e)
        {
            OpenSearch();
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            ClearCurrentPageCode();
        }

        private void toolStripMenuItemDefaultHeader_Click(object sender, EventArgs e)
        {
            SetDefaultHeaderPage();
        }

        private void toolStripMenuItemDefaultSystemReference_Click(object sender, EventArgs e)
        {
            SetDefaultSystemReferencePage();
        }

        private void toolStripMenuItemCompile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void toolStripMenuItemPreview_Click(object sender, EventArgs e)
        {
            Preview();
        }

        private void toolStripMenuItemRun_Click(object sender, EventArgs e)
        {
            RunCode();
        }

        private void toolStripMenuItemPause_Click(object sender, EventArgs e)
        {
            PauseRunningCode();
        }

        private void toolStripMenuItemStop_Click(object sender, EventArgs e)
        {
            StopRunningCode();
        }

        private void OpenMainCode()
        {
            OpenCode?.Invoke();
        }

        private void OpenHeaderCode()
        {
            AddCodePage?.Invoke(CodeManager.HEADER_PAGE);
        }

        private void OpenSystemReferenceCode()
        {
            AddCodePage?.Invoke(CodeManager.SYSTEM_REFERENCE_PAGE);
        }

        private void OpenCustomReferenceCode()
        {
            AddCodePage?.Invoke(CodeManager.CUSTOM_REFERENCE_PAGE);
        }

        private void SetSubMethod()
        {
            AddSubMethod?.Invoke();
        }

        private void RemoveSub()
        {
            if (listViewMethods.FocusedItem != null)
            {
                string strMethodName = listViewMethods.FocusedItem.Text;
                RemoveSubMethod?.Invoke(strMethodName);
            }
        }

        private void OpenSub()
        {
            if (listViewMethods.FocusedItem != null)
            {
                string strMethodName = listViewMethods.FocusedItem.Text;
                OpenSubMethod?.Invoke(strMethodName);
            }
        }

        private void SaveAsMainCode()
        {
            SaveAsCode?.Invoke();
        }

        private void SaveCurrentPageCode()
        {
            SaveCode?.Invoke(CurrentCodePage);
        }

        private void SaveAllPagesCode()
        {
            SaveAllCode?.Invoke();
        }

        private void ClearCurrentPageCode()
        {
            ClearCode?.Invoke();
        }

        private void SetDefaultHeaderPage()
        {
            SetDefaultHeader?.Invoke();
        }

        private void SetDefaultSystemReferencePage()
        {
            SetDefaultSystemReference?.Invoke();
        }

        private void Compile()
        {
            CompileCode?.Invoke();
        }

        private void Preview()
        {
            AddCodePage?.Invoke(CodeManager.GENERATE_PAGE);
        }

        private void RunCode()
        {
            StartCode?.Invoke();
        }

        private void PauseRunningCode()
        {
            PauseCode?.Invoke();
        }

        private void StopRunningCode()
        {
            StopCode?.Invoke();
        }

        private void CloseSpecifiedEditPage(TabPage tabPage)
        {
            if (tabPage == null)
                return;
            CloseSpecifiedPage?.Invoke(tabPage.Name);
        }

        private void CloseOtherEditPages(TabPage tabPage)
        {
            if (tabPage == null)
                return;
            CloseOtherPages?.Invoke(tabPage.Name);
        }

        private void CloseAllEditPages()
        {
            CloseAllPages?.Invoke();
        }

        private void OpenSubMethodPage()
        {
            if (listViewMethods.FocusedItem != null)
            {
                string strMethodName = listViewMethods.FocusedItem.Text;
                OpenSubMethod?.Invoke(strMethodName);
            }
        }

        private void MoveCustomReferenceLibrary()
        {
            MoveCustomReference?.Invoke();
        }

        private void ObtainCustomReferenceLibrary()
        {
            ObtainCustomReference?.Invoke();
        }
    }
}
