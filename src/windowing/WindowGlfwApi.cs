namespace gltest.windowing;

/// <summary>
/// Class to acces GLFW implementation of a window
/// </summary>
public static class WindowGlfwApi
{
    public static void RequestGlfwContext(Window window, GlfwCallback callback)
    {
        window.RequestGlfw(callback);
    }
}