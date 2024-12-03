using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace gltest.Windowing;

public unsafe delegate void RenderCallback(Glfw glfw, GL gl, WindowHandle* window);
public unsafe delegate void GlfwCallback(Glfw glfw, WindowHandle* window);