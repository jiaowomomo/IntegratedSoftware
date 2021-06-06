using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils.Win32
{
    public static class ApplicationHelper
    {
        private const int SW_HIDE = 0;             //隐藏窗口并且激活其它窗口        
        private const int SW_SHOWNORMAL = 1;       //激活和显示窗口。如果窗口是最大化或最小化，恢复其大小和位置。程序不应该在第一次调用ShowWindow时设置此标志   
        private const int SW_SHOWMINIMIZED = 2;    //激活，并按最小化方式显示窗口        
        private const int SW_SHOWMAXIMIZED = 3;    //激活，并按最大化方式显示窗口       
        private const int SW_SHOWNOACTIVATE = 4;   //以最近的大小和位置显示窗口，除了窗口不被激活，其它的类似SW_SHOWNORMAL     
        private const int SW_SHOW = 5;             //在当前位置及大小情况下，激活并显示窗口
        private const int SW_MINIMIZE = 6;	       //最小化窗口，并且按Z序激活下一个窗口
        private const int SW_SHOWMINNOACTIVE = 7;  //最小化窗口。除了窗口不被激活，其它的类似SW_SHOWMINIMIZED
        private const int SW_SHOWNA = 8;           //以当前的大小和位置显示窗口。除了窗口不被激活，其它的类似SW_SHOW
        private const int SW_RESTORE = 9;          //激活并显示窗口，如果窗口最小化或最大化，系统恢复其原来的大小和位置。当恢复最小化窗口时，程序应该使用这个标志     
        private const int SW_SHOWDEFAULT = 10;     //父进程通过 CreateProcess 创建当前进程时，使用此标志来按 STARTUPINFO 结构体中的标志显示窗口
        private const int SW_FORCEMINIMIZE = 11;   //无论拥有窗口的线程是否被挂起，均使窗口最小化。在从其他线程最小化窗口时才使用这个参数。有点类似强制最小化窗口

        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// 窗口是否已最小化         
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("User32.dll")]
        private static extern bool OpenIcon(IntPtr hWnd);

        public static void HandleRunningInstance(Process instance)
        {
            IntPtr hWnd = instance.MainWindowHandle;
            ShowWindowAsync(hWnd, SW_SHOWNORMAL);//还原窗口
            SetForegroundWindow(hWnd);//当到最前端
            ShowWindow(hWnd, SW_SHOWNORMAL);
        }

        public static Process RuningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (Process process in Processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == currentProcess.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否以管理员身份运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
