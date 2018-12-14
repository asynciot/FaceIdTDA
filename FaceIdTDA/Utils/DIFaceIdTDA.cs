using FaceIdTDA.Controllers;
using FaceIdTDA.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FaceIdTDA.Utils
{
    public class DIFaceIdTDA
    {
        private static IServiceCollection serviceCollection;
        private static IServiceProvider serviceProvider;

        static DIFaceIdTDA()
        {
            serviceCollection = new ServiceCollection();
        }

        public static IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }

        public static void InitializeDependencyInjection()
        {
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            #region
            //Entities
            serviceCollection.AddTransient<Image>();

            //Utils
            serviceCollection.AddSingleton<DirectoryTraverse>();

            #endregion

            serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
