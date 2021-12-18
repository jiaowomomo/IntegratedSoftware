using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.Common
{
    public interface ICameraFactory
    {
        ICamera CreateCamera();
    }
}
