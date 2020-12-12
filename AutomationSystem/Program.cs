using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationSystem
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcessesByName("AutomationSystem");
            if (myProcesses.Length > 1) //如果可以获取到知道的进程名大于一个，则说明在此之前已经启动过
            {
                MessageBox.Show("检测到程序已经运行，请先关闭多余的程序和进程!");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SystemMainForm());
        }
    }
}
