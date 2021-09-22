using CommonLibrary.ExtensionUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    public class RunException : Exception
    {
        private RunExceptionType m_exceptionType;

        public RunException(RunExceptionType exceptionType)
        {
            m_exceptionType = exceptionType;
        }

        public override string Message => m_exceptionType.GetDescription();
    }

    public enum RunExceptionType
    {
        [Enum(Description = "没有输入图像")]
        NoInputImage,
        [Enum(Description = "模板查找失败")]
        TemplateLookupFailed,
        [Enum(Description = "两直线数量不相等")]
        TwoLineNumberNotEqual,
        [Enum(Description = "点和直线数量不相等")]
        PointAndLineNumberNotEqual,
        [Enum(Description = "文件路径不存在")]
        FilePathNotExist,
        [Enum(Description = "通讯串口不存在")]
        SerialPortNotExist,
        [Enum(Description = "客户端不存在")]
        SocketClientNotExist,
        [Enum(Description = "服务器不存在")]
        SocketServerNotExist,
        [Enum(Description = "相机不存在")]
        CameraNotExist
    }
}
