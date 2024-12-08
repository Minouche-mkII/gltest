using gltest.Utils.IO;
using Silk.NET.OpenGL;

namespace gltest.Render.Renderers;

public class TriangleRenderer : IRenderer
{
    private readonly float[] _vertices =
    {
        0f, 0.5f, 0f,
        -0.5f, -0.5f, 0f,
        0.5f, -0.5f, 0f
    }; // sommet du triangle

    private uint _vao;
    private uint _shaderProgram;
    
    public void Draw(GL gl)
    {
        gl.UseProgram(_shaderProgram);
        gl.BindVertexArray(_vao);
        gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public unsafe void Load(GL gl)
    {
        _vao = gl.GenVertexArray(); // Vertex Array Object
        var vbo = gl.GenBuffer(); // Vertex Buffer Object
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo); // Bind the VBO to the current GL context
        fixed (float* verticesPtr = &_vertices[0])
        {
            gl.BufferData(BufferTargetARB.ArrayBuffer, (UIntPtr)(_vertices.Length * sizeof(float)),
                verticesPtr, BufferUsageARB.StaticDraw);
        }
        gl.BindVertexArray(_vao); // bind the VAO to our VBO
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        
        gl.EnableVertexArrayAttrib(_vao, 0);
        
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0); // Unbind the VBO to the current GL context
        gl.BindVertexArray(0);
         
        /*
         * Créer un objet VBO avec les vertexes du triangle, et le rajoute dans le VBO crée pour le contexte courrant
         */
        
        // chargement des shaders dans openGL
        _shaderProgram = gl.CreateProgram();
        var vertexShader = gl.CreateShader(ShaderType.VertexShader);
        var fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(vertexShader, FileReader.ReadAllText(@"Shaders/default.vert"));
        gl.ShaderSource(fragmentShader, FileReader.ReadAllText(@"Shaders/default.frag"));
        gl.CompileShader(vertexShader);
        gl.CompileShader(fragmentShader);
        
        gl.AttachShader(_shaderProgram, vertexShader);
        gl.AttachShader(_shaderProgram, fragmentShader);
        
        gl.LinkProgram(_shaderProgram);
        
        gl.DeleteShader(vertexShader);
        gl.DeleteShader(fragmentShader);
    }
    
    public void UnLoad(GL gl)
    {
        gl.DeleteProgram(_shaderProgram);
        gl.DeleteVertexArray(_vao);
    }
}