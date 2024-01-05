
namespace GlobalAPIServices.LoggerService
{
    public interface ILoggerManager
    {
        void LogIfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError (string message);
    }
}
