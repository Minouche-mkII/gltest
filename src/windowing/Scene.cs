using gltest.render;
using gltest.Utils;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public abstract class Scene()
{
    public Window? ParentWindow { get; private set; }

    internal void Enrole(Window? parentWindow)
    {
        ParentWindow = parentWindow;
        WhenEnroled();
    }
    internal void Dismiss()
    {
        ParentWindow = null;
        OnDissmissed();
    }
    
    protected abstract void OnDissmissed();

    protected abstract void WhenEnroled();

    protected void RequestRenderContext(Window.RenderCallback callback)
    {
        ParentWindow?.RequestRenderContext(callback);
    }
    
}