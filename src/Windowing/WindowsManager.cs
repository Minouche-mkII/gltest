using gltest.Utils;
using gltest.Utils.Concurrency;
using gltest.Utils.Logging;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.Windowing;

public static class WindowsManager
{
    public static bool ShutOnMainWindowClosed { get; set; } = true;
    public static bool ShutOnAllWindowsClosed { get; set; } = true;
    private static Window? _mainWindow;
    
    private static readonly List<Window> ActivesWindows = [];
    private static Glfw? _glfw;
    private static GL? _gl;
    private static bool _running = false;

    private static readonly AsyncCallbackQueue MainThreadTaskQueue = new AsyncCallbackQueue();

    /// <summary>
    /// Represent the mainWindow of the application, by default, the application will stop if the main
    /// window is closed. The first window will automaticly be assigned to be the main window
    /// </summary>
    /// <exception cref="InvalidOperationException"> will be thrown if there is no main window.
    /// </exception>
    /// <remarks>
    /// error can happen only if :
    /// <list type="bullet">
    /// <item>no window have been created</item>
    /// <item>if main window was destroyed with ShutOnMainWindowClosed set to false</item>
    /// </list>
    /// </remarks>>
    public static Window MainWindow
    {
        get => _mainWindow ?? throw new InvalidOperationException("no window is the main Window");
        set => _mainWindow = value;
    }
    public static Window[] GetWindows()
    {
        return ActivesWindows.ToArray();
    }

    public static void RunApplication()
    {
        _running = true;
        while (_running)
        {
            Manage();
        }
        CloseGlfwAndFinish();
    }

    private static void Manage()
    {
        _glfw?.PollEvents();
        MainThreadTaskQueue.ExecuteWaitingInstructions();
    }

    private static void CloseGlfwAndFinish()
    {
        _glfw?.Terminate();
        _glfw = null;
        Log.Info("GLFW terminated");
        Log.Info("Application terminated");
        System.Environment.Exit(0);
    }
    
    public static void EndApplication()
    {
        _running = false;
    }

    public static void RequestCallbackForMainThread(Action callbak)
    {
        MainThreadTaskQueue.AddInstructionsToQueue(callbak);
    }
    
    internal static Glfw RegisterNewlyCreatedWindowAndGetApi(Window window)
    {
        if (_glfw == null)
        {
            InitGlfw();
        }
        if (ActivesWindows.Count == 0)
        {
            _mainWindow = window;
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
        _gl = GL.GetApi((name) => _glfw.GetProcAddress(name));
    }
}