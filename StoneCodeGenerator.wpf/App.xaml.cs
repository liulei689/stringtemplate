using HandyControlDemo.Model;
using HandyControlDemo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Rubyer;
using StoneCodeGenerator.Interface.DI;
using StoneCodeGenerator.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        public static string MatchAssemblies = "^StoneCodeGenerator.Service|^StoneCodeGenerator.IService";
        public static List<PlusInterfaceModel> plusInterfaceModels;
        public static List<Type> interfaceTypes;
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
               AddDataService(services);
           }).Build();
        public static IServiceCollection AddDataService(IServiceCollection services)
        {
            #region 依赖注入 批量注入XHS.Service XHS.IService程序集下服务      
            var baseType = typeof(IDependency);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var getFiles = Directory.GetFiles(path, "*.dll").Where(Match); 
            var referencedAssemblies = getFiles.Select(Assembly.LoadFrom).ToList();        
            var ss = referencedAssemblies.SelectMany(o => o.GetTypes());
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToList();
            var implementTypes = types.Where(x => x.IsClass).ToList();
             interfaceTypes = types.Where(x => x.IsInterface).ToList();
            plusInterfaceModels = new List< PlusInterfaceModel>();
            foreach (var interfaceType in interfaceTypes)
            {
                var datas = interfaceType.GetCustomAttributes<DescriptionAttribute>();
                if (datas == null || datas.Count()==0) continue;
                PlusInterfaceModel plusInterfaceModel = new PlusInterfaceModel();
                var name = interfaceType.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault().Description;
                plusInterfaceModel.Name = interfaceType.Name;
                plusInterfaceModel.NameDes= name;
                var methods = interfaceType.GetMethods();
                List<PlusMethodModel> plusMethodModels = new List<PlusMethodModel>();
                for (int j = 0; j < methods.Length; j++)
                {
                    PlusMethodModel plusMethodModel = new PlusMethodModel();
                    var methoddes = methods[j].GetCustomAttributes<DescriptionAttribute>().FirstOrDefault().Description;
                    plusMethodModel.Name = methods[j].Name;
                    plusMethodModel.NameDes= methoddes;
                    plusMethodModels.Add( plusMethodModel );
                }
                plusInterfaceModel.Methods = plusMethodModels;
                plusInterfaceModels.Add( plusInterfaceModel );
            }
            foreach (var implementType in implementTypes)
            {            
                if (typeof(IScopeDependency).IsAssignableFrom(implementType))
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                    if (interfaceType != null)
                        services.AddScoped(interfaceType, implementType);
                }
                else if (typeof(ISingletonDependency).IsAssignableFrom(implementType))
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                    if (interfaceType != null)
                        services.AddSingleton(interfaceType, implementType);
                }
                else
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                    if (interfaceType != null)
                        services.AddTransient(interfaceType, implementType);
                }
            }
            #endregion
            return services;
        }
        public static T GetService<T>()
      where T : class
        {
            
            return _host.Services.GetService(typeof(T)) as T;
        }
        public static object GetServiceByString(string objname)
        {
           var d= interfaceTypes.Find(o => o.Name == objname);
            return _host.Services.GetService(d);
        }
        /// <summary>
        /// 程序集是否匹配
        /// </summary>
        public static bool Match(string assemblyName)
        {
            //return true;
            assemblyName = Path.GetFileName(assemblyName);
            if (assemblyName.StartsWith($"{AppDomain.CurrentDomain.FriendlyName}.Views"))
                return false;
            if (assemblyName.StartsWith($"{AppDomain.CurrentDomain.FriendlyName}.PrecompiledViews"))
                return false;
            if (assemblyName.Contains("Service") && assemblyName.Contains("StoneCodeGenerator."))
                return true;
            return Regex.IsMatch(assemblyName, MatchAssemblies, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
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
