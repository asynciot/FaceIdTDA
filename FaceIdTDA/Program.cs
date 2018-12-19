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
        //public static readonly string REMOTE_SERVER = @"http://33.102.1.103:8100/cmbs/v1/thirdApiService/uploadPic?puid={0}&chan=1&zptime={1}&faceid={2}&facescore=0&x=0&y=0&width=0&height=0&age=0&sex=0&glass=0&end=0&stay=0&picnum=2&gpsEW=E&longitude={3}&gpsNS=N&latitude={4}&remark={5}";
        //public static readonly string FTP_DIRECTORY = @"C:\LiftFpt";
        public static readonly string REMOTE_SERVER = @"http://33.102.2.16:8100/cmbs/v1/thirdApiService/uploadPic?puid={0}&chan=1&zptime={1}&faceid={2}&facescore=0&x=0&y=0&width=0&height=0&age=0&sex=0&glass=0&end=0&stay=0&picnum=2&gpsEW=E&longitude={3}&gpsNS=N&latitude={4}&remark={5}";
        public static readonly string FTP_DIRECTORY = @"D:\LiftFpt";

        static void Main(string[] args)
        {
            DIFaceIdTDA.InitializeDependencyInjection();

            InitializeLog();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 2000;
            timer.Enabled = true;

            Console.WriteLine("AsyncIOT Lift Service started.");
            Console.ReadLine();
            timer.Dispose();
            Console.WriteLine("AsyncIOT Lift Service stopped.");

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
