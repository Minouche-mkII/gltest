using gltest.render;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public class Window
{
    private readonly Glfw _glfw;
    private readonly GL _gl;
    private readonly unsafe WindowHandle* _window;
    public Scene Scene {get; private set;}
    public unsafe delegate void RenderCallback(Glfw glfw, GL gl, WindowHandle* window);
    
    /// <summary>
    /// Create a new window
    /// </summary>
    /// <exception cref="GlfwException">Thrown if GLFW didn't manage to initialize</exception>
    public Window(string title, int height, int width, Scene scene)
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
        Scene = scene;
        Scene.Enrole(this);
    }

    public unsafe void Kill()
    {
        Scene.Dismiss();
        WindowsManager.UnregisterWindow(this);
        _glfw.DestroyWindow(_window);
    }

    public void SetScene(Scene child)
    {
        Scene.Dismiss();
        Scene = child;
        Scene.Enrole(this);
    }
    
    internal unsafe void RequestRenderContext(RenderCallback callback)
    {
        lock(_gl)
        {
            callback(_glfw, _gl, _window);
        }
    }
    
}