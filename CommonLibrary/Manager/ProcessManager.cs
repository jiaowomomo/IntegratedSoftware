using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary.Manager
{
    public class ProcessManager<T>
    {
        private List<T> m_listProcess;
        private List<int> m_listSelectIndex;

        public delegate void ProcessChangedEventHandler(object sender, EventArgs args);
        public ProcessChangedEventHandler OnProcessChanged;

        public delegate void SelectChangedEventHandler(object sender, EventArgs args);
        public SelectChangedEventHandler OnSelectChanged;

        public int ProcessCount { get => m_listProcess.Count; }
        public int SelectedCount { get => m_listSelectIndex.Count; }

        public ProcessManager()
        {
            m_listProcess = new List<T>();
            m_listSelectIndex = new List<int>();
        }

        public void AddProcess(T T_object)
        {
            m_listProcess.Add(T_object);
            OnProcessChanged?.Invoke(null, null);
        }

        public void ReplaceProcess(int index, T T_object)
        {
            if (index >= 0 && index < m_listProcess.Count)
            {
                m_listProcess[index] = T_object;
                OnProcessChanged?.Invoke(null, null);
            }
        }

        public void DeleteProcess(int index)
        {
            m_listProcess.RemoveAt(index);
            OnProcessChanged?.Invoke(null, null);
        }

        public void MoveToTop(int index)
        {
            if ((index > 0) && (index < m_listProcess.Count))
            {
                T temp = m_listProcess[index];
                m_listProcess.Insert(0, temp);
                m_listProcess.RemoveAt(index + 1);
                OnProcessChanged?.Invoke(null, null);
            }
        }

        public void MoveToBottom(int index)
        {
            if (index < m_listProcess.Count)
            {
                T temp = m_listProcess[index];
                m_listProcess.Insert(m_listProcess.Count, temp);
                m_listProcess.RemoveAt(index);
                OnProcessChanged?.Invoke(null, null);
            }
        }

        public void MoveToPrevious(int index)
        {
            if ((index > 0) && (index < m_listProcess.Count))
            {
                T temp = m_listProcess[index];
                m_listProcess[index] = m_listProcess[index - 1];
                m_listProcess[index - 1] = temp;
                OnProcessChanged?.Invoke(null, null);
            }
        }

        public void MoveToNext(int index)
        {
            if ((index > -1) && (index < m_listProcess.Count - 1))
            {
                T temp = m_listProcess[index];
                m_listProcess[index] = m_listProcess[index + 1];
                m_listProcess[index + 1] = temp;
                OnProcessChanged?.Invoke(null, null);
            }
        }

        public bool Save(string fileFullPath)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream(fileFullPath, FileMode.Create))
                {
                    binary.Serialize(stream, m_listProcess);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Load(string fileFullPath)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream(fileFullPath, FileMode.Open))
                {

                    m_listProcess = (List<T>)binary.Deserialize(stream);
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void CreateNewProcessManager()
        {
            m_listProcess = new List<T>();
            OnProcessChanged?.Invoke(null, null);
        }

        public void ClearSelectProcess()
        {
            m_listSelectIndex = new List<int>();
            OnSelectChanged?.Invoke(null, null);
        }

        public void AddSelectIndex(int nIndex)
        {
            if ((nIndex >= 0) && (nIndex < m_listProcess.Count))
            {
                m_listSelectIndex.Add(nIndex);
                OnSelectChanged?.Invoke(null, null);
            }
        }

        public void SetSelectIndex(List<int> selectedList)
        {
            m_listSelectIndex = selectedList;
            OnSelectChanged?.Invoke(null, null);
        }

        public T GetProcessByIndex(int nIndex)
        {
            if ((nIndex >= 0) && (nIndex < m_listProcess.Count))
            {
                return m_listProcess[nIndex];
            }
            return default(T);
        }

        public int GetSelectedProcessIndex(int nIndex)
        {
            if ((nIndex >= 0) && (nIndex < m_listSelectIndex.Count))
            {
                return m_listSelectIndex[nIndex];
            }
            return -1;
        }
    }
}
