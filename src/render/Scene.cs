using gltest.Utils;
using gltest.windowing;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.render;

public class Scene : WindowChild
{
    public int MaxFramesPerSecond { get; set; }
    private bool _alive;
    private readonly Thread _renderThread;
    public bool Paused;
    
    public Scene() : base()
    {
        MaxFramesPerSecond = 24;
        _alive = false;
        _renderThread = new Thread(new ThreadStart((() =>
        {
            while (_alive)
            {
                Thread.Sleep(1000 / MaxFramesPerSecond);
                RequestRenderContext();
            }
        })));
        unsafe
        {
            OnRender(Render);
        }
    }
    
    public void Start()
    {
        if (ParentWindow == null)
        {
            Log.Warning("Can't start a Scene whithout parent");
            return;
        }
        _alive = true;
        _renderThread.Start();
    }

    public void Stop()
    {
        _alive = false;
    }
    

    private unsafe void Render(Glfw glfw, GL gl, WindowHandle* windowHandle)
    {
        Console.WriteLine("Appelé");
    }

    protected override void OnDissmissed()
    {
        Stop();
    }
}