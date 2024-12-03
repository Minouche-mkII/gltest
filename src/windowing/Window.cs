using System.Numerics;
using gltest.render;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public class Window
{
    private readonly Glfw _glfw;
    private readonly GL _gl;
    private readonly unsafe WindowHandle* _window;
    private Scene _scene;
    
    /// <summary>
    /// Create a new window
    /// </summary>
    /// <exception cref="GlfwException">Thrown if GLFW didn't manage to initialize</exception>
    private Window(string title, int height, int width, Scene scene)
    {
        _glfw = WindowsManager.RegisterNewlyCreatedWindowAndGetApi(this);
        unsafe
        {
            _window = _glfw.CreateWindow(width, height, title, null, null);
            
            if (_window == null)
                throw new GlfwException("Could not create window");
            
            _glfw.SetWindowCloseCallback(_window, window =>
            {
                Kill();
            });
        }
        _gl = WindowsManager.GetGl();
        _scene = scene;
        _scene.Enrole(this);
    }

    public Window(string title, int height, int width, Scene scene, bool center = true) :
        this(title, height, width, scene)
    {
        if (center)
        {
            Center();
        }
    }

    public Window(string title, int height, int width, int x, int y, Scene scene) :
        this(title, height, width, scene)
    {
        SetPosition(x, y);
    }
    
    public Scene Scene
    {
        get => _scene;
        set
        {
            _scene.Dismiss();
            _scene = value;
            _scene.Enrole(this);
        }
    }

    public unsafe void Center()
    {
        WindowsManager.RequestCallbackForMainThread(new Task(() =>
        {
            _glfw.GetWindowSize(_window, out var windowWidth, out var windowHeight);
            _glfw.GetMonitorWorkarea(
                _glfw.GetPrimaryMonitor(),
                out var monitorX,
                out var monitorY,
                out var monitorWidth,
                out var monitorHeight);
            var windowX = monitorWidth / 2 - (windowWidth / 2);
            var windowY = monitorHeight / 2 - (windowHeight / 2);
            _glfw.SetWindowPos(_window, monitorX + windowX , monitorY + windowY);
        }));
    }

    public unsafe void SetPosition(int x, int y)
    { 
        WindowsManager.RequestCallbackForMainThread(new Task(() =>
        {
            _glfw.GetMonitorPos(_glfw.GetPrimaryMonitor(), out var left, out var top);
            _glfw.SetWindowPos(_window, left + x, top + y);
        }));
    }
    
    public unsafe void Kill()
    {
        _scene.Dismiss();
        WindowsManager.UnregisterWindow(this);
        _glfw.DestroyWindow(_window);
    }
    
    internal unsafe void RequestRenderContext(RenderCallback callback)
    {
        lock(_gl)
        {
            _glfw.MakeContextCurrent(_window);
            callback(_glfw, _gl, _window);
        }
    }

    internal unsafe void RequestGlfw(GlfwCallback callback)
    {
        callback(_glfw, _window);
    }
}