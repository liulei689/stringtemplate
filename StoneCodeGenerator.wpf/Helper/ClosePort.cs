using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace HandyControlDemo.Helper
{
    public class ClosePort
    {
        public static void  ClosethePort(int port) {
            bool isElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

            if (!isElevated)
            {
                // 如果当前用户不是管理员，则使用管理员权限重新启动应用程序
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Verb = "runas";
                startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                startInfo.Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1));
                try
                {
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    // 处理异常
                }
                Environment.Exit(0);
            }
            else
            {
                // 如果当前用户已经是管理员，则继续执行代码
                // ...
            }

            // 获取所有正在运行的进程
            Process[] processes = Process.GetProcesses();

            // 遍历所有进程
            foreach (Process process in processes)
            {
                try
                {
                    // 获取进程的所有打开的端口
                    var connections = IPGlobalProperties.GetIPGlobalProperties()
                        .GetActiveTcpListeners()
                        .Where(c => c.Port == port)
                        .ToArray();

                    // 如果进程打开了指定的端口，则关闭该进程
                    if (connections.Length > 0)
                    {
                        process.Kill();
                    }
                }
                catch (Exception ex)
                {
                    // 处理异常
                }
            }
        }
    }
}
