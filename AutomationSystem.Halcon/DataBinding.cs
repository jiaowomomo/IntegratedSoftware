using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public class DataBinding
    {
        public string DataName = "";
        public string DataType = "INT";
        public int DataSourceIndex = 0;
        public string DataSourceName = "";
    }
}
