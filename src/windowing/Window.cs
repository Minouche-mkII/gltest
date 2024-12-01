using gltest.render;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public class Window
{
    private readonly Glfw _glfw;
    private readonly GL _gl;
    private readonly unsafe WindowHandle* _window;
    private WindowChild _child;
    public unsafe delegate void RenderCallback(Glfw glfw, GL gl, WindowHandle* window);

    internal event RenderCallback? OnRender;
    
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
        _child = new Scene();
        _child.Enrole(this);
    }

    public unsafe void Kill()
    {
        _child.Dismiss();
        WindowsManager.UnregisterWindow(this);
        _glfw.DestroyWindow(_window);
    }

    public void SetChild(WindowChild child)
    {
        _child.Dismiss();
        _child = child;
        _child.Enrole(this);
    }
    
    internal unsafe void RequestRenderContext()
    {
        lock(_gl)
        {
            OnRender?.Invoke(_glfw, _gl, _window);
        }
    }
    
}