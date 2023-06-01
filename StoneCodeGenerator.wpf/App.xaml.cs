using HandyControlDemo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HandyControlDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SingleInstanceCheck();
            ShowSplashScreen();
        }
        private static readonly IHost _host = Host
           .CreateDefaultBuilder()
           .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
           .ConfigureServices((context, services) =>
           {
               // App Host
               services.AddHostedService<ApplicationHostService>();
              // AddDataService(services);
           }).Build();
        void ShowSplashScreen()
        {
            var splashScreen = new SplashScreen("Resource/Image/1.jpg");
            splashScreen.Show(true);
        }
        static Mutex _mutex;
        static Task _task;
        // Mutex allows us to know whether another instance is already open
        const string MutexId = "captura-mutex-304bae7c-e520-4bfe-a308-c99476062091";

        // EventWaitHandle allows us to communicate to a already running instance
        const string EventWaitHandleId = "captura-wait-304bae7c-e520-4bfe-a308-c99476062091";

        public static void SingleInstanceCheck()
        {
            _mutex = new Mutex(true, MutexId, out var createdNew);

            if (!createdNew)
            {
                // Bring the already running instance to the foreground
                var handle = CreateWaitHandle();
                handle.Set();

                // Exit duplicate instance
                Environment.Exit(0);
            }
        }

        static EventWaitHandle CreateWaitHandle()
        {
            return new EventWaitHandle(false, EventResetMode.AutoReset, EventWaitHandleId);
        }
        public static void StartListening(Action Callback)
        {
            // First instance listens for events from other instances in a loop
            _task = Task.Run(() =>
            {
                var waitHandle = CreateWaitHandle();

                while (true)
                {
                    waitHandle.WaitOne();

                    // Callback should bring first instance to foreground
                    Callback.Invoke();
                }
            });
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }
}
