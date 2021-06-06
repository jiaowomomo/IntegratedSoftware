using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    public class RunException : Exception
    {
        private int m_exceptionNumber;

        public RunException(int num)
        {
            m_exceptionNumber = num;
        }

        public override string Message => ((RunExceptionType)m_exceptionNumber).ToString();
    }

    public enum RunExceptionType
    {
        没有输入图像 = 1,
        模板查找失败 = 2,
        两直线数量不相等 = 3,
        点和直线数量不相等 = 4,
        文件路径不存在 = 5,
        通讯串口不存在 = 6,
        客户端不存在 = 7,
        服务器不存在 = 8,
        选择相机不存在 = 9
    }
}
