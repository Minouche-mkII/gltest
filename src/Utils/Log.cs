namespace gltest.Utils;

public static class Log
{
    private static readonly List<Tuple<Type, string>> RegisteredLogs = [];
    private static bool _consoleEntered = false;

    public enum Type
    {
        Error,
        Warning,
        Info
    }

    private static readonly Dictionary<Type, string> StringOf = new Dictionary<Type, string>
    {
        { Type.Info, "Info" },
        { Type.Warning, "Warning" },
        { Type.Error, "Error" }
    };

    public static void WriteInConsole()
    {
        _consoleEntered = true;
    }

    public static void StopWriteInConsole()
    {
        _consoleEntered = false;
    }

    public static void RegisterLog(Type type, string log)
    {
        RegisteredLogs.Add(new Tuple<Type, string>(type, log));
        if (_consoleEntered)
        {
            Console.WriteLine($"- LOG | {StringOf[type]} : {log}");
        }
    }

    public static void Info(string log)
    {
        RegisterLog(Type.Info, log);
    }
    
    public static void Warning(string log)
    {
        RegisterLog(Type.Warning, log);
    }
    
    public static void Error(string log)
    {
        RegisterLog(Type.Error, log);
    }
    
}