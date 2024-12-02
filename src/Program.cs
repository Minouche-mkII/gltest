using gltest.render;
using gltest.Utils;
using gltest.windowing;

namespace gltest;

internal static class Program
{
    public static void Main(string[] args)
    {
        Log.WriteInConsole();
        var mainScene = new DefaultScene();
        var window = new Window("HelloWorld", 700, 1000, mainScene);
        
        mainScene.Start();
        
        WindowsManager.RunApplication();
    }
}

