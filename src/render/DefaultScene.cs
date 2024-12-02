using gltest.Utils;
using gltest.windowing;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.render;

public class DefaultScene : Scene
{
    private const int DefaultMaxFramePerSecond = 24;
    public int MaxFramesPerSecond
    {
        get => _renderThread.MaxCallPerSeconds;
        set => _renderThread.MaxCallPerSeconds = value;
    }
    private readonly RefreshThread _renderThread;

    public DefaultScene(int maxFramesPerSecond)
    {
        _renderThread = new RefreshThread(maxFramesPerSecond, (() =>
        {
            unsafe
            {
                RequestRenderContext(Render);
            }
        }));
    }
    
    private unsafe void Render(Glfw glfw, GL gl, WindowHandle* windowHandle)
    {
        
    }

    private void Init()
    {
        
    }

    public DefaultScene() : this(DefaultMaxFramePerSecond) { }

    public void Start()
    {
        if (ParentWindow == null)
        {
            Log.Warning("Can't start a Scene whithout parent");
            return;
        }
        try
        {
            _renderThread.Start();
        }
        catch (ThreadStateException ex)
        {
            Log.Warning("The scene has already been started");
        }
    }

    public void Pause()
    {
        _renderThread.Pause();
    }

    public void Resume()
    {
        _renderThread.Resume();
    }

    protected override void OnDissmissed()
    {
        _renderThread.Pause();
    }

    protected override void WhenEnroled()
    {
        Init();
    }
}