using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.AutoCircleCalibration
{
    class Circle
    {
        public double Row { get; set; } = 0;
        public double Column { get; set; } = 0;
        public double Radius { get; set; } = 10;
        public Circle(double row,double column,double radius)
        {
            Row = row;
            Column = column;
            Radius = radius;
        }
    }
}
