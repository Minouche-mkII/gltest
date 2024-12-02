using gltest.render;
using gltest.Utils;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.windowing;

public abstract class DefaultScene()
{
    public Window? ParentWindow { get; private set; }
    private Window.RenderCallback? _renderCallBack;

    internal void Enrole(Window? parentWindow)
    {
        ParentWindow = parentWindow;
        ParentWindow!.OnRender += _renderCallBack;
    }
    internal void Dismiss()
    {
        if (ParentWindow != null) ParentWindow.OnRender -= _renderCallBack;
        ParentWindow = null;
        OnDissmissed();
    }
    
    protected abstract void OnDissmissed();

    protected void RequestRenderContext()
    {
        ParentWindow?.RequestRenderContext();
    }

    protected void OnRender(Window.RenderCallback callback)
    {
        if (ParentWindow != null) ParentWindow.OnRender += callback;
        _renderCallBack = callback;
    }
}