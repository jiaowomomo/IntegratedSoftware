using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Manager
{
    public enum ErrorReason
    {
        Null,
        KeyNotFound,
        KeyExist,
        IndexOutOfRange,
        ConversionFailure
    }

    public class ProcessManagerResult<T>
    {
        public bool OK { get; set; } = true;
        public ErrorReason Error { get; set; } = ErrorReason.Null;
        public T GetProcessManager { get; set; }
    }
}
