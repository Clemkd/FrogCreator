using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Log;

public class Console
{
    public static bool DEBUG = true;
    public static bool LOGS_ENABLED = false;
    public static readonly Console log = new Console();
    private static readonly string LOGGER_NAME = "frogLogs";
    private StreamWriter? _logWriter;

    public Console()
    {
        try
        {
            _logWriter = new StreamWriter("logs", append: true);
        }
        catch (Exception e)
        {
            System.Console.Error.WriteLine(e);
        }
    }

    public void error(FrogException exception)
    {
        if (DEBUG)
            System.Console.Error.WriteLine($"[{exception.GetType().Name}][{DateTime.UtcNow}] {exception.StackTrace}");
        if (LOGS_ENABLED)
            _logWriter?.WriteLine(exception.StackTrace);
    }
}
