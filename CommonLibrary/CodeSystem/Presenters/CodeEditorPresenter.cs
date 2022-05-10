using CommonLibrary.CodeSystem.Controls;
using CommonLibrary.CodeSystem.Presenters.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem.Presenters
{
    public class CodeEditorPresenter
    {
        private const string MODIFY_FLAG = "*";

        private ICodeEditor m_view;
        private CustomReferenceForm m_customReferenceForm = null;

        public CodeEditorPresenter(ICodeEditor view)
        {
            m_view = view;

            m_view.ClearCode += ClearCode;
            m_view.OpenCode += OpenCode;
            m_view.SaveAsCode += SaveAsCode;
            m_view.SaveCode += SaveCode;
            m_view.SaveAllCode += SaveAllCode;
            m_view.CompileCode += CompileCode;
            m_view.StopCode += StopCode;
            m_view.PauseCode += PauseCode;
            m_view.StartCode += StartCode;
            m_view.PreviousSearch += PreviousSearch;
            m_view.NextSearch += NextSearch;
            m_view.AddCodePage += AddCodePage;
            m_view.AddSubMethod += AddSubMethod;
            m_view.RemoveSubMethod += RemoveSubMethod;
            m_view.OpenSubMethod += OpenSubMethod;
            m_view.CloseSpecifiedPage += CloseSpecifiedPage;
            m_view.CloseOtherPages += CloseOtherPages;
            m_view.CloseAllPages += CloseAllPages;
            m_view.SetDefaultHeader += SetDefaultHeader;
            m_view.SetDefaultSystemReference += SetDefaultSystemReference;
            m_view.MoveCustomReference += MoveCustomReference;
            m_view.ObtainCustomReference += ObtainCustomReference;
        }

        private void ObtainCustomReference()
        {
            if (m_customReferenceForm == null)
            {
                m_customReferenceForm = new CustomReferenceForm();
            }
            m_customReferenceForm.Show();
            m_customReferenceForm.Focus();
        }

        private void MoveCustomReference()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "动态库|*.dll";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string strMessage = string.Empty;
                    foreach (string strFileName in ofd.SafeFileNames)
                    {
                        string strNewDLLFullPath = Path.Combine(Application.StartupPath, strFileName);
                        try
                        {
                            if (File.Exists(strNewDLLFullPath))
                            {
                                File.Copy(ofd.FileName, strNewDLLFullPath, true);
                                strMessage += $"{strFileName} 成功\r\n";
                            }
                            else
                            {
                                strNewDLLFullPath = Path.Combine(CodeManager.Instance.ThirdPartyDLLDirectory, strFileName);
                                File.Copy(ofd.FileName, strNewDLLFullPath, true);
                                strMessage += $"{strFileName} 成功 \r\n";
                            }
                        }
                        catch (Exception ex)
                        {
                            strMessage += $"{strFileName} 失败: {ex.Message}\r\n";
                        }
                    }
                    strMessage = $"增加引用动态库\r\n{strMessage}";
                    ShowMessage(strMessage);
                }
            }
        }

        private void SaveAsCode()
        {
            using (SaveFileDialog ofd = new SaveFileDialog())
            {
                ofd.Filter = "脚本文件|*.main";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (m_view.MainCodePage != null)
                    {
                        using (StreamWriter sw = new StreamWriter(ofd.FileName))
                        {
                            CodeEditPage codeEditPage = m_view.MainCodePage.Controls[0] as CodeEditPage;
                            if (codeEditPage != null)
                            {
                                sw.Write(codeEditPage.Text);
                                CodeManager.Instance.MainCodeFullPath = ofd.FileName;
                                CodeManager.Instance.SaveHistoryCodePath();
                                m_view.CodePathLabel.Text = $"当前脚本: {CodeManager.Instance.MainCodeFullPath}";
                                RemoveModifyFlag(m_view.MainCodePage);
                                ShowMessage($"{ofd.FileName}保存成功.");
                            }
                        }
                    }
                }
            }
        }

        private void SetDefaultSystemReference()
        {
            if (m_view.CurrentCodePage != null)
            {
                if (m_view.CurrentCodePage.Name.Equals(CodeManager.SYSTEM_REFERENCE_PAGE, StringComparison.OrdinalIgnoreCase))
                {
                    CodeEditPage codeEditPage = m_view.CurrentCodePage.Controls[0] as CodeEditPage;
                    if (codeEditPage != null)
                    {
                        codeEditPage.Text = CodeManager.DEFAULT_SYSTEM_REFERENCE;
                        ShowMessage("恢复默认系统Reference文件.");
                    }
                }
            }
        }

        private void SetDefaultHeader()
        {
            if (m_view.CurrentCodePage != null)
            {
                if (m_view.CurrentCodePage.Name.Equals(CodeManager.HEADER_PAGE, StringComparison.OrdinalIgnoreCase))
                {
                    CodeEditPage codeEditPage = m_view.CurrentCodePage.Controls[0] as CodeEditPage;
                    if (codeEditPage != null)
                    {
                        codeEditPage.Text = CodeManager.DEFAULT_HEADER;
                        ShowMessage("恢复默认Header代码.");
                    }
                }
            }
        }

        private void SaveAllCode()
        {
            for (int i = 0; i < m_view.TabControlCode.TabPages.Count; i++)
            {
                TabPage tabPage = m_view.TabControlCode.TabPages[i];
                SaveCode(tabPage);
            }
        }

        private void CloseAllPages()
        {
            int nTabPageCount = m_view.TabControlCode.TabPages.Count;
            string strMessage = string.Empty;
            for (int i = 0; i < nTabPageCount; i++)
            {
                if (m_view.TabControlCode.TabPages[i].Text.EndsWith(MODIFY_FLAG))
                {
                    strMessage += $"{m_view.TabControlCode.TabPages[i].Name}\r\n";
                }
            }
            if (!string.IsNullOrEmpty(strMessage))
            {
                strMessage = $"是否保存对以下各项的更改?\r\n{strMessage}";
                switch (MessageBox.Show(strMessage, "脚本编辑", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        SaveAllCode();
                        break;
                    case DialogResult.No:
                        break;
                }
            }
            for (int i = nTabPageCount - 1; i > 0; i--)
            {
                m_view.TabControlCode.TabPages.RemoveAt(i);
            }
        }

        private void CloseOtherPages(string strMethodName)
        {
            int nTabPageCount = m_view.TabControlCode.TabPages.Count;
            string strMessage = string.Empty;
            for (int i = 0; i < nTabPageCount; i++)
            {
                if (m_view.TabControlCode.TabPages[i].Name.Equals(strMethodName, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (m_view.TabControlCode.TabPages[i].Text.EndsWith(MODIFY_FLAG))
                {
                    strMessage += $"{m_view.TabControlCode.TabPages[i].Name}\r\n";
                }
            }
            if (!string.IsNullOrEmpty(strMessage))
            {
                strMessage = $"是否保存对以下各项的更改?\r\n{strMessage}";
                switch (MessageBox.Show(strMessage, "脚本编辑", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        {
                            for (int i = 0; i < nTabPageCount; i++)
                            {
                                if (m_view.TabControlCode.TabPages[i].Name.Equals(strMethodName, StringComparison.OrdinalIgnoreCase))
                                    continue;
                                SaveCode(m_view.TabControlCode.TabPages[i]);
                            }
                        }
                        break;
                    case DialogResult.No:
                        break;
                }
            }
            for (int i = nTabPageCount - 1; i > 0; i--)
            {
                if (m_view.TabControlCode.TabPages[i].Name.Equals(strMethodName, StringComparison.OrdinalIgnoreCase))
                    continue;
                m_view.TabControlCode.TabPages.RemoveAt(i);
            }
        }

        private void CloseSpecifiedPage(string strMethodName)
        {
            if (strMethodName.Equals(CodeManager.MAIN_PAGE, StringComparison.OrdinalIgnoreCase))
                return;
            if (m_view.TabControlCode.TabPages.IndexOfKey(strMethodName) != -1)
            {
                string strMessage = string.Empty;
                if (m_view.TabControlCode.TabPages[strMethodName].Text.EndsWith(MODIFY_FLAG))
                {
                    strMessage += $"是否保存对以下各项的更改?\r\n{m_view.TabControlCode.TabPages[strMethodName].Name}\r\n";
                    switch (MessageBox.Show(strMessage, "脚本编辑", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.Yes:
                            SaveCode(m_view.TabControlCode.TabPages[strMethodName]);
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                m_view.TabControlCode.TabPages.RemoveByKey(strMethodName);
                ShowMessage($"关闭{strMethodName}.");
            }
        }

        private void OpenSubMethod(string strMethodName)
        {
            AddCodePage(strMethodName);
            ShowMessage($"打开{strMethodName}.");
        }

        private void RemoveSubMethod(string strMethodName)
        {
            if (MessageBox.Show($"是否删除该Sub代码{strMethodName}", "代码编辑", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (IsExistMethodCodePage(strMethodName))
                {
                    m_view.TabControlCode.TabPages.RemoveByKey(strMethodName);
                }
                m_view.ListViewMethod.Items.RemoveByKey(strMethodName);
                CodeManager.Instance.SubMethods.Remove(strMethodName);
                try
                {
                    File.Delete(Path.Combine(CodeManager.Instance.CodePartDirectory, strMethodName));
                }
                catch
                {
                }
                ShowMessage($"删除{strMethodName}.");
            }
        }

        private void AddSubMethod()
        {
            using (RenameForm renameForm = new RenameForm())
            {
                if (renameForm.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(renameForm.MethodName))
                    {
                        string strMethodName = renameForm.MethodName;
                        strMethodName += strMethodName.EndsWith(CodeManager.SUB_SUFFIX) ? string.Empty : CodeManager.SUB_SUFFIX;
                        if (CodeManager.Instance.SubMethods.Contains(strMethodName))
                            return;
                        m_view.ListViewMethod.Items.Add(strMethodName, strMethodName, null);
                        string strDirectory = CodeManager.Instance.CodePartDirectory;
                        if (!Directory.Exists(strDirectory))
                        {
                            Directory.CreateDirectory(strDirectory);
                        }
                        string strFullPath = Path.Combine(strDirectory, strMethodName);
                        using (StreamWriter sw = new StreamWriter(strFullPath))
                        {
                            switch (renameForm.SubType)
                            {
                                case SubType.Method:
                                    {
                                        sw.WriteLine($"private void {strMethodName.Replace(CodeManager.SUB_SUFFIX, string.Empty)}()");
                                        sw.WriteLine("{");
                                        sw.WriteLine("");
                                        sw.WriteLine("}");
                                    }
                                    break;
                                case SubType.Class:
                                    {
                                        sw.WriteLine($"private class {strMethodName.Replace(CodeManager.SUB_SUFFIX, string.Empty)}");
                                        sw.WriteLine("{");
                                        sw.WriteLine("");
                                        sw.WriteLine("}");
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        CodeManager.Instance.SubMethods.Add(strMethodName);
                        AddCodePage(strMethodName);
                        ShowMessage($"增加{strMethodName}.");
                    }
                }
            }
        }

        private void AddCodePage(string strMethodName)
        {
            if (string.IsNullOrEmpty(strMethodName))
                return;

            if (!IsExistMethodCodePage(strMethodName))
            {
                string strFullPath;
                if (strMethodName.Equals(CodeManager.MAIN_PAGE, StringComparison.OrdinalIgnoreCase))
                {
                    strFullPath = CodeManager.Instance.MainCodeFullPath;
                }
                else
                {
                    strFullPath = Path.Combine(CodeManager.Instance.CodePartDirectory, strMethodName);
                }
                CodeEditPage codeEditPage = new CodeEditPage();
                LoadCode(codeEditPage, strFullPath);
                if (strMethodName.Equals(CodeManager.GENERATE_PAGE, StringComparison.OrdinalIgnoreCase))
                {
                    codeEditPage.ReadOnly = true;
                }
                codeEditPage.CanEdit = true;
                TabPage tabPage = new TabPage();
                tabPage.Name = strMethodName;
                tabPage.Text = strMethodName;
                codeEditPage.CodeChanged += () =>
                  {
                      SetModifyFlag(tabPage);
                  };
                tabPage.Controls.Add(codeEditPage);
                m_view.TabControlCode.TabPages.Add(tabPage);
                m_view.TabControlCode.SelectedTab = tabPage;
            }
            else
            {
                if (m_view.TabControlCode.TabPages.IndexOfKey(strMethodName) != -1)
                {
                    TabPage tabPage = m_view.TabControlCode.TabPages[strMethodName];
                    m_view.TabControlCode.SelectedTab = tabPage;
                    if (strMethodName.Equals(CodeManager.GENERATE_PAGE, StringComparison.OrdinalIgnoreCase))
                    {
                        CodeEditPage codeEditPage = tabPage.Controls[0] as CodeEditPage;
                        string strFullPath = Path.Combine(CodeManager.Instance.CodePartDirectory, strMethodName);
                        codeEditPage.ReadOnly = false;
                        LoadCode(codeEditPage, strFullPath);
                        codeEditPage.ReadOnly = true;
                    }
                }
            }
        }

        private void NextSearch()
        {
            SearchManager.Find(true);
        }

        private void PreviousSearch()
        {
            SearchManager.Find(false);
        }

        private void StartCode()
        {
            SaveAllCode();
            if (CodeManager.Instance.RunCode())
            {
                ShowMessage("开始运行.");
            }
            else
            {
                ShowMessage(CodeManager.Instance.GetErrors());
            }
        }

        private void PauseCode()
        {
            CodeManager.Instance.PauseCode();
            ShowMessage("暂停运行.");
        }

        private void StopCode()
        {
            CodeManager.Instance.StopCode();
            ShowMessage("停止运行.");
        }

        private void CompileCode()
        {
            SaveAllCode();
            CodeManager.Instance.Compile();
            AddCodePage(CodeManager.GENERATE_PAGE);
            ShowMessage(CodeManager.Instance.GetErrors());
        }

        private void SaveCode(TabPage tabPage)
        {
            if (tabPage != null)
            {
                string strFullPath;
                string strSavePath = tabPage.Name;
                if (strSavePath.Equals(CodeManager.GENERATE_PAGE, StringComparison.OrdinalIgnoreCase))
                    return;
                if (strSavePath.Equals(CodeManager.MAIN_PAGE, StringComparison.OrdinalIgnoreCase))
                {
                    strFullPath = CodeManager.Instance.MainCodeFullPath;
                }
                else
                {
                    string strDirectory = CodeManager.Instance.CodePartDirectory;
                    if (!Directory.Exists(strDirectory))
                    {
                        Directory.CreateDirectory(strDirectory);
                    }
                    strFullPath = Path.Combine(strDirectory, strSavePath);
                }
                using (StreamWriter sw = new StreamWriter(strFullPath))
                {
                    CodeEditPage codeEditPage = tabPage.Controls[0] as CodeEditPage;
                    if (codeEditPage != null)
                    {
                        sw.Write(codeEditPage.Text);
                        RemoveModifyFlag(tabPage);
                        ShowMessage($"{strSavePath}保存成功.");
                    }
                }
            }
        }

        private void OpenCode()
        {
            if (m_view.MainCodePage == null)
                return;
            string strMessage = string.Empty;
            if (m_view.MainCodePage.Text.EndsWith(MODIFY_FLAG))
            {
                strMessage += $"是否保存对以下各项的更改?\r\n{m_view.MainCodePage.Name}\r\n";
                switch (MessageBox.Show(strMessage, "脚本编辑", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        SaveCode(m_view.MainCodePage);
                        break;
                    case DialogResult.No:
                        break;
                }
            }
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "脚本文件|*.main";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (m_view.MainCodePage != null)
                    {
                        CodeEditPage codeEditPage = m_view.MainCodePage.Controls[0] as CodeEditPage;
                        LoadCode(codeEditPage, ofd.FileName);
                        codeEditPage.CanEdit = true;
                        CodeManager.Instance.MainCodeFullPath = ofd.FileName;
                        CodeManager.Instance.SaveHistoryCodePath();
                        m_view.CodePathLabel.Text = $"当前脚本: {CodeManager.Instance.MainCodeFullPath}";
                        RemoveModifyFlag(m_view.MainCodePage);
                    }
                }
            }
        }

        private void ClearCode()
        {
            if (m_view.CurrentCodePage != null)
            {
                CodeEditPage codeEditPage = m_view.CurrentCodePage.Controls[0] as CodeEditPage;
                if (codeEditPage != null)
                {
                    codeEditPage.Text = string.Empty;
                    ShowMessage("清空代码.");
                }
            }
        }

        private bool IsExistMethodCodePage(string strMethodName)
        {
            return m_view.TabControlCode.TabPages.IndexOfKey(strMethodName) != -1;
        }

        private void ShowMessage(string strMessage)
        {
            m_view.MessageTextBox.AppendText($"{DateTime.Now}: {strMessage}\r\n");
        }

        private void LoadCode(CodeEditPage codeEditPage, string strCodeFullPath)
        {
            if (codeEditPage == null || !File.Exists(strCodeFullPath))
                return;
            try
            {
                codeEditPage.CanEdit = false;
                codeEditPage.Text = File.ReadAllText(strCodeFullPath);
            }
            catch
            {
            }
        }

        private void SetModifyFlag(TabPage tabPage)
        {
            if (!tabPage.Text.EndsWith(MODIFY_FLAG))
            {
                tabPage.Text += MODIFY_FLAG;
            }
        }

        private void RemoveModifyFlag(TabPage tabPage)
        {
            if (tabPage.Text.EndsWith(MODIFY_FLAG))
            {
                tabPage.Text = tabPage.Text.Remove(tabPage.Text.Length - 1);
            }
        }
    }
}
