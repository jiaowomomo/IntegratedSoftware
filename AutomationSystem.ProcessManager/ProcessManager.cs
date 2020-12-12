using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace AutomationSystem.Manager
{
    public class ProcessManager<T>
    {
        private List<T> _m_listProcess;

        private List<T> m_listProcess
        {
            get { return _m_listProcess; }
            set
            {
                _m_listProcess = value;
                if (OnProcessChanged != null)
                {
                    OnProcessChanged(null, new EventArgs());
                }
            }
        }

        public ProcessManager()
        {
            m_listProcess = new List<T>();
            m_listSelectIndex = new List<int>();
        }

        public void AddProcess(T T_object)
        {
            List<T> list = m_listProcess.Select(item => item).ToList();
            list.Add(T_object);
            m_listProcess = list;
        }

        public void DeleteProcess(int index)
        {
            List<T> list = m_listProcess.Select(item => item).ToList();
            list.RemoveAt(index);
            m_listProcess = list;
        }

        public void MoveToTop(int index)
        {
            if ((index != 0) && (index < m_listProcess.Count))
            {
                List<T> list = m_listProcess.Select(item => item).ToList();
                T temp = list[index];
                list.Insert(0, temp);
                list.RemoveAt(index + 1);
                m_listProcess = list;
            }
        }

        public void MoveToBottom(int index)
        {
            if (index < m_listProcess.Count)
            {
                List<T> list = m_listProcess.Select(item => item).ToList();
                T temp = list[index];
                list.Insert(list.Count, temp);
                list.RemoveAt(index);
                m_listProcess = list;
            }
        }

        public void MoveToProvious(int index)
        {
            if ((index > 0) && (index < m_listProcess.Count))
            {
                List<T> list = m_listProcess.Select(item => item).ToList();
                T temp = list[index];
                list[index] = list[index - 1];
                list[index - 1] = temp;
                m_listProcess = list;
            }
        }

        public void MoveToNext(int index)
        {
            if ((index > -1) && (index < m_listProcess.Count - 1))
            {
                List<T> list = m_listProcess.Select(item => item).ToList();
                T temp = list[index];
                list[index] = list[index + 1];
                list[index + 1] = temp;
                m_listProcess = list;
            }
        }

        public List<T> ProcessList
        {
            get
            {
                return m_listProcess;
            }
        }

        public bool Save(string fileName)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream(fileName, FileMode.Create))
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

        public bool Load(string fileName)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream(fileName, FileMode.Open))
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

        public void CreateNewProcess()
        {
            m_listProcess = new List<T>();
        }

        private List<int> _m_listSelectIndex;

        private List<int> m_listSelectIndex
        {
            get { return _m_listSelectIndex; }
            set
            {
                _m_listSelectIndex = value;
                if (OnSelectChanged != null)
                {
                    OnSelectChanged(null, new EventArgs());
                }
            }
        }

        public void ClearSelectProcess()
        {
            m_listSelectIndex = new List<int>();
        }

        public void AddSelectIndex(int nIndex)
        {
            if ((nIndex >= 0) && (nIndex < m_listProcess.Count))
            {
                List<int> list = m_listSelectIndex.Select(item => item).ToList();
                list.Add(nIndex);
                m_listSelectIndex = list;
            }
        }

        public void SetSelectIndex(List<int> int_list)
        {
            m_listSelectIndex = int_list;
        }

        public T GetSelectProcess(int nIndex)
        {
            if ((nIndex >= 0) && (nIndex < m_listProcess.Count))
            {
                return m_listProcess[nIndex];
            }
            return default(T);
        }

        public List<int> SelectIndexList
        {
            get
            {
                return m_listSelectIndex;
            }
        }

        public delegate void ProcessChangedEventHandler(object sender, EventArgs args);
        public ProcessChangedEventHandler OnProcessChanged;

        public delegate void SelectChangedEventHandler(object sender, EventArgs args);
        public SelectChangedEventHandler OnSelectChanged;
    }
}
