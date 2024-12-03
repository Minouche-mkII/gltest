using gltest.Render;
using gltest.Utils;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.Windowing;

/// <summary>
/// 
/// </summary>
/// <remarks>a scene can be attributed to only one window at the time</remarks>
public abstract class Scene()
{
    public Window? ParentWindow { get; private set; }

    internal unsafe void Enrole(Window? parentWindow, Glfw glfw, GL gl, WindowHandle* windowHandle)
    {
        ParentWindow = parentWindow;
        WhenEnroled(glfw, gl, windowHandle);
    }
    internal void Dismiss()
    {
        ParentWindow = null;
        OnDissmissed();
    }
    
    protected abstract void OnDissmissed();

    protected abstract unsafe void WhenEnroled(Glfw glfw, GL gl, WindowHandle* windowHandle);

    protected void RequestRenderContext(RenderCallback callback)
    {
        ParentWindow?.RequestRenderContext(callback);
    }
    
}