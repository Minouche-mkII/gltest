using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace gltest;

public class Window
{
    private readonly IWindow _window;
    private GL? _gl;

    public Window(string title, int width, int height, bool center = true)
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.Title = title;
        windowOptions.Size = new Vector2D<int>(width, height);
        windowOptions.Position = new Vector2D<int>(0, 0);
        _window = Silk.NET.Windowing.Window.Create(windowOptions);

        _window.Load += (() =>
        {
            _gl = _window.CreateOpenGL();
            if (center)
                _window.Center();
        });

        _window.Update += WindowOnUpdate;
        _window.Render += WindowOnRender;
    }

    public void SetSize(int width, int height)
    {
        _window.Size = new Vector2D<int>(width, height);
    }

    public void Start()
    {
        _window.Run();
    }

    private void WindowOnRender(double obj)
    {
        
    }

    private void WindowOnUpdate(double obj)
    {
        
    }
}