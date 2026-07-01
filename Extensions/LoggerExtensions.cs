using System.Runtime.CompilerServices;

namespace bookpj.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogRequest(
            this ILogger logger,
            string className,
            object? data = null,
            [CallerMemberName] string methodName = "")
        {
            if (data != null)
            {
                logger.LogInformation("[{Class}] - [{Method}] - Request triggered with data: {@Data}", className, methodName, data);
            }
            else
            {
                logger.LogInformation("[{Class}] - [{Method}] - Request triggered", className, methodName);
            }
        }

        public static void LogResponse(
            this ILogger logger,
            string className,
            object? responseData = null,
            [CallerMemberName] string methodName = "")
        {
            logger.LogInformation("[{Class}] - [{Method}] - Response: {@ResponseData}", className, methodName, responseData ?? "Success");
        }
    }
}

