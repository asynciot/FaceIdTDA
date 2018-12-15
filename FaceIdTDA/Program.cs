using FaceIdTDA.Controllers;
using FaceIdTDA.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Timers;

namespace FaceIdTDA
{
    class Program
    {
        public static readonly string REMOTE_SERVER = "http://www.baidu.com/service/upload_pic.php?puid=1";
        public static readonly string FTP_DIRECTORY = @"D:\Cloud\FTPDir\";

        static void Main(string[] args)
        {
            DIFaceIdTDA.InitializeDependencyInjection();

            InitializeLog();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 3000;
            timer.Enabled = true;

            Console.WriteLine("AsyncIOT Lift Service started.");
            Console.ReadLine();
            timer.Dispose();
            Console.WriteLine("AsyncIOT Lift Service stopped.");

            //IServiceProvider serviceProvider = DIFaceIdTDA.GetServiceProvider();
            //ILogger logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            //logger.LogE("abcdfg");

        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            IServiceProvider serviceProvider = DIFaceIdTDA.GetServiceProvider();
            var directoryTraverse = serviceProvider.GetRequiredService<DirectoryTraverse>();
            directoryTraverse.Retrieve();
        } 

        public static void InitializeLog()
        {
            IServiceProvider serviceProvider = DIFaceIdTDA.GetServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("NLog.config");
        }
    }
}
