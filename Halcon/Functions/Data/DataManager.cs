using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public class DataManager
    {
        private Dictionary<string, int> m_listInputInt = new Dictionary<string, int>();//整形输入
        private Dictionary<string, List<int>> m_listInputIntArray = new Dictionary<string, List<int>>();//整形数组输入
        private Dictionary<string, double> m_listInputDouble = new Dictionary<string, double>();//双精度输入
        private Dictionary<string, List<double>> m_listInputDoubleArray = new Dictionary<string, List<double>>();//双精度数组输入

        private Dictionary<string, int> m_listOutputInt = new Dictionary<string, int>();//整形输出
        private Dictionary<string, List<int>> m_listOutputIntArray = new Dictionary<string, List<int>>();//整形数组输出
        private Dictionary<string, double> m_listOutputDouble = new Dictionary<string, double>();//双精度输出
        private Dictionary<string, List<double>> m_listOutputDoubleArray = new Dictionary<string, List<double>>();//双精度数组输出

        private List<DataBinding> m_listDataBinding = new List<DataBinding>();

        public int DataBindingCount { get => m_listDataBinding.Count; }

        //添加数据
        public void AddInputInt(string inputName)
        {
            m_listInputInt.Add(inputName, new int());
            m_listDataBinding.Add(null);
        }

        public void AddInputIntArray(string inputNme)
        {
            m_listInputIntArray.Add(inputNme, new List<int>());
            m_listDataBinding.Add(null);
        }

        public void AddInputDouble(string inputName)
        {
            m_listInputDouble.Add(inputName, new double());
            m_listDataBinding.Add(null);
        }

        public void AddInputDoubleArray(string inputName)
        {
            m_listInputDoubleArray.Add(inputName, new List<double>());
            m_listDataBinding.Add(null);
        }

        public void AddOutputInt(string outputName)
        {
            m_listOutputInt.Add(outputName, new int());
        }

        public void AddOutputIntArray(string outputName)
        {
            m_listOutputIntArray.Add(outputName, new List<int>());
        }

        public void AddOutputDouble(string outputName)
        {
            m_listOutputDouble.Add(outputName, new double());
        }

        public void AddOutputDoubleArray(string outputName)
        {
            m_listOutputDoubleArray.Add(outputName, new List<double>());
        }

        //设置数据
        public void SetInputInt(string inputName, int value)
        {
            m_listInputInt[inputName] = value;
        }

        public void SetInputIntArray(string inputName, List<int> value)
        {
            m_listInputIntArray[inputName] = value;
        }

        public void SetInputDouble(string inputName, double value)
        {
            m_listInputDouble[inputName] = value;
        }

        public void SetInputDoubleArray(string inputName, List<double> value)
        {
            m_listInputDoubleArray[inputName] = value;
        }

        public void SetOutputInt(string outputName, int value)
        {
            m_listOutputInt[outputName] = value;
        }

        public void SetOutputIntArray(string outputName, List<int> value)
        {
            m_listOutputIntArray[outputName] = value;
        }

        public void SetOutputDouble(string outputName, double value)
        {
            m_listOutputDouble[outputName] = value;
        }

        public void SetOutputDoubleArray(string outputName, List<double> value)
        {
            m_listOutputDoubleArray[outputName] = value;
        }

        //获取数据
        public int GetInputInt(string inputName)
        {
            return m_listInputInt[inputName];
        }

        public List<int> GetInputIntArray(string inputName)
        {
            return m_listInputIntArray[inputName];
        }

        public double GetInputDouble(string inputName)
        {
            return m_listInputDouble[inputName];
        }

        public List<double> GetInputDoubleArray(string inputName)
        {
            return m_listInputDoubleArray[inputName];
        }

        public int GetOutputInt(string outputName)
        {
            return m_listOutputInt[outputName];
        }

        public List<int> GetOutputIntArray(string outputName)
        {
            return m_listOutputIntArray[outputName];
        }

        public double GetOutputDouble(string outputName)
        {
            return m_listOutputDouble[outputName];
        }

        public List<double> GetOutputDoubleArray(string outputName)
        {
            return m_listOutputDoubleArray[outputName];
        }

        //获取数据名称
        public List<string> GetInputIntNames()
        {
            return m_listInputInt.Keys.ToList();
        }

        public List<string> GetInputIntArrayNames()
        {
            return m_listInputIntArray.Keys.ToList();
        }

        public List<string> GetInputDoubleNames()
        {
            return m_listInputDouble.Keys.ToList();
        }

        public List<string> GetInputDoubleArrayNames()
        {
            return m_listInputDoubleArray.Keys.ToList();
        }

        public List<string> GetOutputIntNames()
        {
            return m_listOutputInt.Keys.ToList();
        }

        public List<string> GetOutputIntArrayNames()
        {
            return m_listOutputIntArray.Keys.ToList();
        }

        public List<string> GetOutputDoubleNames()
        {
            return m_listOutputDouble.Keys.ToList();
        }

        public List<string> GetOutputDoubleArrayNames()
        {
            return m_listOutputDoubleArray.Keys.ToList();
        }

        //设置绑定数据
        public void SetDataBinding(int dataIndex, string dataName, DataType dataType, int sourceIndex, string sourceName)
        {
            if ((dataIndex < 0) || (dataIndex >= m_listDataBinding.Count))
            {
                return;
            }
            m_listDataBinding[dataIndex] = new DataBinding() { DataName = dataName, DataType = dataType, DataSourceIndex = sourceIndex, DataSourceName = sourceName };
        }

        public DataBinding GetDataBinding(int dataIndex)
        {
            if ((dataIndex < 0) || (dataIndex >= m_listDataBinding.Count))
            {
                return null;
            }
            else
            {
                return m_listDataBinding[dataIndex];
            }
        }

        //清空数据
        public void ClearData()
        {
            foreach (var item in m_listOutputInt.ToList())
            {
                m_listOutputInt[item.Key] = new int();
            }
            foreach (var item in m_listOutputIntArray.ToList())
            {
                m_listOutputIntArray[item.Key] = new List<int>();
            }
            foreach (var item in m_listOutputDouble.ToList())
            {
                m_listOutputDouble[item.Key] = new double();
            }
            foreach (var item in m_listOutputDoubleArray.ToList())
            {
                m_listOutputDoubleArray[item.Key] = new List<double>();
            }
        }
    }
}
