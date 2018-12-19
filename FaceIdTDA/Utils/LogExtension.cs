using Microsoft.Extensions.Logging;
using System;

namespace FaceIdTDA.Utils
{
    public static class LogExtension
    {
        public static void LogI(this ILogger logger, string message)
        {
            logger.LogInformation(message);
        }

        public static void LogI(this ILogger logger, Exception exception)
        {
            logger.LogInformation(exception, exception.Message);
        }

        public static void LogW(this ILogger logger, Exception exception)
        {
            logger.LogWarning(exception, exception.Message);
        }

        public static void LogE(this ILogger logger, Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }

        public static void LogE(this ILogger logger, string message)
        {
            logger.LogError(message);
        }
    }
}
