using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    public enum DataType
    {
        INT,
        INTARRAY,
        DOUBLE,
        DOUBLEARRAY
    }

    [Serializable]
    public class DataBinding
    {
        private string m_strDataName = string.Empty;
        private DataType m_dataType = Functions.DataType.INT;
        private int m_nDataSourceIndex = 0;
        private string m_strDataSourceName = string.Empty;

        public string DataName { get => m_strDataName; set => m_strDataName = value; }
        public DataType DataType { get => m_dataType; set => m_dataType = value; }
        public int DataSourceIndex { get => m_nDataSourceIndex; set => m_nDataSourceIndex = value; }
        public string DataSourceName { get => m_strDataSourceName; set => m_strDataSourceName = value; }
    }
}
