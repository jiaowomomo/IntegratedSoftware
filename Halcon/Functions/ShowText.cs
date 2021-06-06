using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    public class ShowText
    {
        public double PositionX { get; set; } = 0;

        public double PositionY { get; set; } = 0;

        public string ShowColor { get; set; } = "green";

        public string ShowContent { get; set; } = string.Empty;

        public ShowText(double posX, double posY, string text, string color = "green")
        {
            PositionX = posX;
            PositionY = posY;
            ShowColor = color;
            ShowContent = text;
        }
    }
}
