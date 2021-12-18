using Camera.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.MindVision
{
    class MindVisionFactory : ICameraFactory
    {
        public ICamera CreateCamera()
        {
            return new MindVisionCamera();
        }
    }
}
