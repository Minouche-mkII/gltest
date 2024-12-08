using gltest.Render.Renderers;
using gltest.Utils;
using gltest.Utils.Concurrency;
using gltest.Utils.Logging;
using gltest.Windowing;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.Render;

public class DefaultScene : Scene
{
    private Glfw? _glfw;
    private GL? _gl;
    private unsafe WindowHandle* _windowHandle;
    private IRenderer _renderer;
    public int MaxFramesPerSecond
    {
        get => _renderThread.MaxCallPerSeconds;
        set => _renderThread.MaxCallPerSeconds = value;
    }
    private readonly RefreshThread _renderThread;

    public DefaultScene(int maxFramesPerSecond = 24)
    {
        _renderThread = new RefreshThread(maxFramesPerSecond, Render);
        _renderer = new TriangleRenderer();
    }
    
    private void Render()
    {
        _gl!.Clear(ClearBufferMask.ColorBufferBit);
        _renderer!.Draw(_gl);
        
        unsafe
        {
            _glfw!.SwapBuffers(_windowHandle);
        }
    }

    // from render thread only
    private unsafe void Init()
    {
        _glfw!.MakeContextCurrent(_windowHandle);
        _gl!.ClearColor(0.05f, 0.07f, 0.1f, 1.0f);
        _renderer.Load(_gl);
    }

    protected override unsafe void WhenEnroled(Glfw glfw, GL gl, WindowHandle* windowHandle)
    {
        _glfw = glfw;
        _gl = gl;
        _windowHandle = windowHandle;
        /* Make the init method be called from the render thread. As OpenGL require to be called
         from the same thread. Init Task will be added to the queue and be executed when the Thread
         resume*/
        _renderThread.ExecutePonctualInstructions(Init);
    }
    
    protected override void OnDissmissed()
    {
        _renderThread.Pause();
    }
    
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

    public void End()
    {
        _renderThread.End();
        _renderer.UnLoad(_gl!);
    }

    public void Pause()
    {
        _renderThread.Pause();
    }

    public void Resume()
    {
        if (ParentWindow == null)
        {
            Log.Warning("Can't start a Scene whithout parent");
            return;
        }
        _renderThread.Resume();
    }
}