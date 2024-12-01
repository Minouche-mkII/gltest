using gltest.Utils;
using gltest.windowing;

namespace gltest;

internal static class Program
{
    public static void Main(string[] args)
    {
        Log.WriteInConsole();
        var window = new Window("HelloWorld", 700, 1000);
        window.Start();
        WindowsManager.RunApplication();
    }
}

