using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using CommonLibrary.Manager;
using CommonLibrary.ExtensionUtils;

namespace UIControl.Common
{
    public partial class ObjectListView<T> : UserControl
    {
        public ProcessManager<T> m_listProcess;

        public ObjectListView(ProcessManager<T> listProcess)
        {
            InitializeComponent();
            m_listProcess = listProcess;
            SetListView();
        }

        private void SetListView()
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Clear();
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var item in fields)
            {
                Attribute attribute = item.GetCustomAttribute(typeof(ShowerAttribute));
                if (attribute != null)
                {
                    this.listView1.Columns.Add(((ShowerAttribute)attribute).Name);
                }
            }
            foreach (var item in propertyInfos)
            {
                Attribute attribute = item.GetCustomAttribute(typeof(ShowerAttribute));
                if (attribute != null)
                {
                    this.listView1.Columns.Add(((ShowerAttribute)attribute).Name);
                }
            }
        }

        public void UpdateListView(object sender, EventArgs args)
        {
            this.listView1.Items.Clear();
            for (int i = 0; i < m_listProcess.ProcessCount; i++)
            {
                T t_object = m_listProcess.GetProcessByIndex(i);
                Type type = t_object.GetType();
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                ListViewItem listViews = new ListViewItem();
                for (int j = 0; j < this.listView1.Columns.Count; j++)
                {
                    foreach (var item in fields)
                    {
                        Attribute attribute = item.GetCustomAttribute(typeof(ShowerAttribute));
                        if (attribute != null)
                        {
                            if (((ShowerAttribute)attribute).Name == this.listView1.Columns[j].Text)
                            {
                                listViews.SubItems.Add(item.GetValue(t_object).ToString());
                            }
                        }
                    }
                    foreach (var item in propertyInfos)
                    {
                        Attribute attribute = item.GetCustomAttribute(typeof(ShowerAttribute));
                        if (attribute != null)
                        {
                            if (((ShowerAttribute)attribute).Name == this.listView1.Columns[j].Text)
                            {
                                listViews.SubItems.Add(item.GetValue(t_object).ToString());
                            }
                        }
                    }
                }
                listViews.SubItems.RemoveAt(0);
                this.listView1.Items.Add(listViews);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            m_listProcess.MoveToPrevious(nSeletedIndex);
        }

        private int nSeletedIndex = -1;

        //public void AddSelectHandle(EventHandler eventHandler)
        //{
        //    this.listView1.Click += eventHandler;
        //}

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_listProcess.MoveToNext(nSeletedIndex);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            m_listProcess.MoveToTop(nSeletedIndex);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            m_listProcess.MoveToBottom(nSeletedIndex);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            m_listProcess.DeleteProcess(nSeletedIndex);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            m_listProcess.CreateNewProcessManager();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                nSeletedIndex = listView1.SelectedItems[0].Index;
                List<int> int_list = new List<int>();
                int_list.Add(nSeletedIndex);
                OnListSelect(int_list);
            }
        }

        public delegate void ListSelectEventHandler(List<int> list);
        public ListSelectEventHandler OnListSelect;
    }
}
