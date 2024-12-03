using gltest.Render;
using gltest.Utils;
using gltest.Utils.Logging;
using gltest.Windowing;

namespace gltest;

internal static class Program
{
    public static void Main(string[] args)
    {
        Log.WriteInConsole();
        var mainScene = new DefaultScene();
        var scene2 = new DefaultScene();
        var window = new Window("HelloWorld", mainScene, 700, 1000);
        var window2 = new Window("HelloWorld2", scene2, 500, 500);
        mainScene.Start();
        scene2.Start();
        WindowsManager.RunApplication();
    }
}

