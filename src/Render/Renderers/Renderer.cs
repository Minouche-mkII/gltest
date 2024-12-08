using Silk.NET.OpenGL;

namespace gltest.Render.Renderers;

public interface IRenderer
{
    public void Draw(GL gl);

    public void UnLoad(GL gl);

    public void Load(GL gl);
}