using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public class DataManager
    {
        Dictionary<string, int> m_listInputInt = new Dictionary<string, int>();//整形输入
        Dictionary<string, List<int>> m_listInputIntVector = new Dictionary<string, List<int>>();//整形数组输入
        Dictionary<string, double> m_listInputDouble = new Dictionary<string, double>();//双精度输入
        Dictionary<string, List<double>> m_listInputDoubleVector = new Dictionary<string, List<double>>();//双精度数组输入

        Dictionary<string, int> m_listOutputInt = new Dictionary<string, int>();//整形输出
        Dictionary<string, List<int>> m_listOutputIntVector = new Dictionary<string, List<int>>();//整形数组输出
        Dictionary<string, double> m_listOutputDouble = new Dictionary<string, double>();//双精度输出
        Dictionary<string, List<double>> m_listOutputDoubleVector = new Dictionary<string, List<double>>();//双精度数组输出

        List<DataBinding> m_listDataBinding = new List<DataBinding>();

        //添加数据
        public void AddInputInt(string inputName)
        {
            m_listInputInt.Add(inputName, new int());
            m_listDataBinding.Add(null);
        }

        public void AddInputIntVector(string inputNme)
        {
            m_listInputIntVector.Add(inputNme, new List<int>());
            m_listDataBinding.Add(null);
        }

        public void AddInputDouble(string inputName)
        {
            m_listInputDouble.Add(inputName, new double());
            m_listDataBinding.Add(null);
        }

        public void AddInputDoubleVector(string inputName)
        {
            m_listInputDoubleVector.Add(inputName, new List<double>());
            m_listDataBinding.Add(null);
        }

        public void AddOutputInt(string outputName)
        {
            m_listOutputInt.Add(outputName, new int());
        }

        public void AddOutputIntVector(string outputName)
        {
            m_listOutputIntVector.Add(outputName, new List<int>());
        }

        public void AddOutputDouble(string outputName)
        {
            m_listOutputDouble.Add(outputName, new double());
        }

        public void AddOutputDoubleVector(string outputName)
        {
            m_listOutputDoubleVector.Add(outputName, new List<double>());
        }

        //设置数据
        public void SetInputInt(string inputName, int value)
        {
            m_listInputInt[inputName] = value;
        }

        public void SetInputIntVector(string inputName, List<int> value)
        {
            m_listInputIntVector[inputName] = value;
        }

        public void SetInputDouble(string inputName, double value)
        {
            m_listInputDouble[inputName] = value;
        }

        public void SetInputDoubleVector(string inputName, List<double> value)
        {
            m_listInputDoubleVector[inputName] = value;
        }

        public void SetOutputInt(string outputName, int value)
        {
            m_listOutputInt[outputName] = value;
        }

        public void SetOutputIntVector(string outputName, List<int> value)
        {
            m_listOutputIntVector[outputName] = value;
        }

        public void SetOutputDouble(string outputName, double value)
        {
            m_listOutputDouble[outputName] = value;
        }

        public void SetOutputDoubleVector(string outputName, List<double> value)
        {
            m_listOutputDoubleVector[outputName] = value;
        }

        //获取数据
        public int GetInputInt(string inputName)
        {
            return m_listInputInt[inputName];
        }

        public List<int> GetInputIntVector(string inputName)
        {
            return m_listInputIntVector[inputName];
        }

        public double GetInputDouble(string inputName)
        {
            return m_listInputDouble[inputName];
        }

        public List<double> GetInputDoubleVector(string inputName)
        {
            return m_listInputDoubleVector[inputName];
        }

        public int GetOutputInt(string outputName)
        {
            return m_listOutputInt[outputName];
        }

        public List<int> GetOutputIntVector(string outputName)
        {
            return m_listOutputIntVector[outputName];
        }

        public double GetOutputDouble(string outputName)
        {
            return m_listOutputDouble[outputName];
        }

        public List<double> GetOutputDoubleVector(string outputName)
        {
            return m_listOutputDoubleVector[outputName];
        }

        //获取数据名称
        public List<string> GetInputIntNames()
        {
            return m_listInputInt.Keys.ToList();
        }

        public List<string> GetInputIntVectorNames()
        {
            return m_listInputIntVector.Keys.ToList();
        }

        public List<string> GetInputDoubleNames()
        {
            return m_listInputDouble.Keys.ToList();
        }

        public List<string> GetInputDoubleVectorNames()
        {
            return m_listInputDoubleVector.Keys.ToList();
        }

        public List<string> GetOutputIntNames()
        {
            return m_listOutputInt.Keys.ToList();
        }

        public List<string> GetOutputIntVectorNames()
        {
            return m_listOutputIntVector.Keys.ToList();
        }

        public List<string> GetOutputDoubleNames()
        {
            return m_listOutputDouble.Keys.ToList();
        }

        public List<string> GetOutputDoubleVectorNames()
        {
            return m_listOutputDoubleVector.Keys.ToList();
        }

        //设置绑定数据
        public void SetDataBinding(int dataIndex, string dataName, string dataType, int sourceIndex, string sourceName)
        {
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

        public int GetDataBindingCount()
        {
            return m_listDataBinding.Count;
        }

        //清空数据
        public void ClearData()
        {
            foreach (var item in m_listOutputInt.ToList())
            {
                m_listOutputInt[item.Key] = new int();
            }
            foreach (var item in m_listOutputIntVector.ToList())
            {
                m_listOutputIntVector[item.Key] = new List<int>();
            }
            foreach (var item in m_listOutputDouble.ToList())
            {
                m_listOutputDouble[item.Key] = new double();
            }
            foreach (var item in m_listOutputDoubleVector.ToList())
            {
                m_listOutputDoubleVector[item.Key] = new List<double>();
            }
        }
    }
}
