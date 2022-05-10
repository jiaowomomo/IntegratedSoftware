using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.CodeSystem
{
    public enum CodeStatus
    {
        Idle,
        Run,
        Pause,
        AbnormalStop
    }

    public enum SubType
    {
        Method = 0,
        Class
    }
}
