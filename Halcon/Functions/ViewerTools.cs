using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    public enum ViewerTools
    {
        Arrow = 1,
        ZoomIn = 2,
        ZoomOut = 4,
        Hand = 8,
        Rectangle = 16,
        RotateRectangle = 32,
        Circle = 64,
        Ellipse = 128
    }
}
