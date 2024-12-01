using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public class Window
{
    private readonly Glfw _glfw;
    private readonly GL _gl;
    private readonly unsafe WindowHandle* _window;
    private bool _alive;
    public int MaxFramesPerSecond { get; set; }
    
    /// <summary>
    /// Create a new window
    /// </summary>
    /// <exception cref="GlfwException">Thrown if GLFW didn't manage to initialize</exception>
    public Window(string title, int height, int width)
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
        MaxFramesPerSecond = 24;
        _alive = true;
    }

    public void Start()
    {
        new Thread(new ThreadStart((() =>
        {
            while (_alive)
            {
                Thread.Sleep(1000/MaxFramesPerSecond);
                Render();
            }
        }))).Start();
    }

    public unsafe void Kill()
    {
        _alive = false;
        WindowsManager.UnregisterWindow(this);
        _glfw.DestroyWindow(_window);
    }
    
    private unsafe void Render()
    {
        lock (_gl)
        {
            _glfw.MakeContextCurrent(_window);
            _gl.Clear(ClearBufferMask.ColorBufferBit);
            _glfw.SwapBuffers(_window);
        }
    }
}