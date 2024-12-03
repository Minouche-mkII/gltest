using System.Numerics;
using gltest.Render;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gltest.Windowing;

public class Window
{
    protected readonly Glfw Glfw;
    private readonly GL _gl;
    private readonly unsafe WindowHandle* _window;
    private Scene _scene;
    
    /// <summary>
    /// Create a new window
    /// </summary>
    /// <exception cref="GlfwException">Thrown if GLFW didn't manage to initialize</exception>
    /// <remarks>a scene can be attributed to only one window at the time</remarks>
    public unsafe Window(string title, Scene scene, int height = 700, int width = 800,
        Window? shareResourcesWith = null, bool center = true)
    {
        //TODO: implement resource sharing
        Glfw = WindowsManager.RegisterNewlyCreatedWindowAndGetApi(this);
        
        _window = Glfw.CreateWindow(width, height, title, null, null);
        
        if (_window == null)
            throw new GlfwException("Could not create window");
        
        Glfw.SetWindowCloseCallback(_window, window =>
        {
            Kill();
        });
        
        _gl = WindowsManager.GetGl();
        _scene = scene;
        _scene.Enrole(this, this.Glfw, _gl, _window);

        if (center)
        {
            Center();
        }
    }
    
    public Scene Scene
    {
        get => _scene;
        set
        {
            unsafe
            {
                _scene.Dismiss();
                _scene = value;
                _scene.Enrole(this, this.Glfw, _gl, _window);
            }
        }
    }

    public unsafe void Center()
    {
        WindowsManager.RequestCallbackForMainThread(() =>
        {
            Glfw.GetWindowSize(_window, out var windowWidth, out var windowHeight);
            Glfw.GetMonitorWorkarea(
                Glfw.GetPrimaryMonitor(),
                out var monitorX,
                out var monitorY,
                out var monitorWidth,
                out var monitorHeight);
            var windowX = monitorWidth / 2 - (windowWidth / 2);
            var windowY = monitorHeight / 2 - (windowHeight / 2);
            Glfw.SetWindowPos(_window, monitorX + windowX , monitorY + windowY);
        });
    }

    public unsafe void SetPosition(int x, int y)
    { 
        WindowsManager.RequestCallbackForMainThread(() =>
        {
            Glfw.GetMonitorPos(Glfw.GetPrimaryMonitor(), out var left, out var top);
            Glfw.SetWindowPos(_window, left + x, top + y);
        });
    }
    
    public unsafe void Kill()
    {
        _scene.Dismiss();
        WindowsManager.UnregisterWindow(this);
        Glfw.DestroyWindow(_window);
    }
    
    internal unsafe void RequestRenderContext(RenderCallback callback)
    {
        callback(Glfw, _gl, _window);
    }
}