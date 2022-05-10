using CommonLibrary.CodeSystem.Controls;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem
{
    internal class SearchManager
    {
        private static int m_nLastSearchIndex = 0;
        private static bool m_bIsNext = true;

        public static CodeEditPage TextArea { get; set; } = null;
        public static TextBox SearchBox { get; set; } = null;

        public static string LastSearch { get; private set; } = string.Empty;

        public static void Find(bool bIsNext)
        {
            if (TextArea == null || SearchBox == null)
                return;

            if (!LastSearch.Equals(SearchBox.Text, StringComparison.OrdinalIgnoreCase))
            {
                LastSearch = SearchBox.Text;
                m_nLastSearchIndex = 0;
            }
            if (LastSearch.Length > 0)
            {
                TextArea.SearchFlags = SearchFlags.None;
                int nTextLength = TextArea.TextLength;
                if (!bIsNext.Equals(m_bIsNext))
                {
                    m_bIsNext = bIsNext;
                    m_nLastSearchIndex = nTextLength - m_nLastSearchIndex + LastSearch.Length;
                }

                if (bIsNext)
                {
                    //设置搜索范围
                    TextArea.TargetStart = m_nLastSearchIndex;
                    TextArea.TargetEnd = nTextLength;

                    if (TextArea.SearchInTarget(LastSearch) == -1)
                    {
                        m_nLastSearchIndex = 0;
                        TextArea.TargetStart = m_nLastSearchIndex;
                        TextArea.TargetEnd = nTextLength;

                        if (TextArea.SearchInTarget(LastSearch) == -1)
                        {
                            TextArea.ClearSelections();
                            return;
                        }
                    }

                    m_nLastSearchIndex = TextArea.TargetEnd;
                }
                else
                {
                    //反转内容
                    string strText = new string(TextArea.Text.Reverse().ToArray());
                    string strSearch = new string(LastSearch.Reverse().ToArray());

                    int nIndex = strText.IndexOf(strSearch, m_nLastSearchIndex, StringComparison.OrdinalIgnoreCase);
                    if (nIndex == -1)
                    {
                        m_nLastSearchIndex = 0;

                        nIndex = strText.IndexOf(strSearch, m_nLastSearchIndex, StringComparison.OrdinalIgnoreCase);
                        if (nIndex == -1)
                        {
                            TextArea.ClearSelections();
                            return;
                        }
                        else
                        {
                            //反转索引
                            TextArea.TargetStart = nTextLength - (nIndex + strSearch.Length);
                            TextArea.TargetEnd = nTextLength - nIndex;
                        }
                    }
                    else
                    {
                        TextArea.TargetStart = nTextLength - (nIndex + strSearch.Length);
                        TextArea.TargetEnd = nTextLength - nIndex;
                    }

                    m_nLastSearchIndex = nIndex + strSearch.Length;
                }

                //设置选择内容
                TextArea.SetSelection(TextArea.TargetEnd, TextArea.TargetStart);
                TextArea.ScrollCaret();
            }

            SearchBox.Focus();
        }
    }
}
