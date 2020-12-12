using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    public class ShowText
    {
        public double m_dbPosX = 0;

        public double m_dbPosY = 0;

        public string m_showColor = "green";

        public string m_strText = "";

        public ShowText(double posX, double posY, string text, string color = "green")
        {
            m_dbPosX = posX;
            m_dbPosY = posY;
            m_showColor = color;
            m_strText = text;
        }
    }
}
