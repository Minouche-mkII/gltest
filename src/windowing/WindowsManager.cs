using gltest.Utils;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public static class WindowsManager
{
    public static bool ShutOnMainWindowClosed { get; set; } = true;
    public static bool ShutOnAllWindowsClosed { get; set; } = true;
    
    private static readonly List<Window> ActivesWindows = [];
    private static Window? _mainWindow;
    private static Glfw? _glfw;
    private static GL? _gl;
    private static bool _running = false;
    
    public static Window[] GetWindows()
    {
        return ActivesWindows.ToArray();
    }

    public static Window? GetMainWindow()
    {
        return _mainWindow;
    }

    public static void RunApplication()
    {
        _running = true;
        while (_running)
        {
            _glfw?.PollEvents();
        }
        _glfw?.Terminate();
        _glfw = null;
        Log.Info("GLFW terminated");
        Log.Info("Application terminated");
        System.Environment.Exit(0);
    }

    public static void StopApplication()
    {
        _running = false;
    }

    public static void SetMainWindow(Window window)
    {
        _mainWindow = window;
    }
    
    internal static Glfw RegisterNewlyCreatedWindowAndGetApi(Window window)
    {
        if (_glfw == null)
        {
            InitGlfw();
        }
        if (ActivesWindows.Count == 0)
        {
            SetMainWindow(window);
        }
        ActivesWindows.Add(window);
        return _glfw!;
    }
    
    /// <summary>
    /// Remove Windows when destroyed, if there are no windows remaining,
    /// terminate automaticly GLFW to properly end the program
    /// </summary>
    /// <param name="window"></param>
    internal static void UnregisterWindow(Window window)
    {
        ActivesWindows.Remove(window);

        if (window.Equals(_mainWindow))
        {
            _mainWindow = null;
            if (ShutOnMainWindowClosed)
            {
                _running = false;
                return;
            }
        }
        if (ShutOnAllWindowsClosed && ActivesWindows.Count == 0)
        {
            _running = false;
        }
    }
    
    internal static GL GetGl()
    {
        return _gl ?? throw new InvalidOperationException("GLFW not correctly initialized");
    }

    private static void InitGlfw()
    {
        _glfw = Glfw.GetApi();
        if (!_glfw.Init())
        {
            _glfw = null;
            Log.Error("Failed to initialize GLFW API");
            throw new Exception("Failed to initialize GLFW API");
        }
        Log.Info("GLFW API initialized");
        _glfw.SetErrorCallback((error, description) =>
        {
            Log.Error($"GLFW {error} , {description}");
        });
        _gl = GL.GetApi(_glfw.GetProcAddress);
    }
}