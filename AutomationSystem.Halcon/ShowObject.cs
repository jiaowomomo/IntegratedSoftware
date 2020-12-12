using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    public class ShowObject
    {
        public HObject m_object;

        public string m_showColor = "red";

        public string m_strDrawMode = "margin";

        public ShowObject(HObject hObject, string color = "red", string drawMode = "margin")
        {
            m_object = hObject;
            m_showColor = color;
            m_strDrawMode = drawMode;
        }
    }
}
