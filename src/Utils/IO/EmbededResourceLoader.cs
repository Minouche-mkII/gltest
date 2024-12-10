using System.Reflection;

namespace gltest.Utils.IO;

public class EmbededResourceLoader
{
    public static string ReadAllText(string filePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var truc = assembly.GetManifestResourceNames();
        using var stream = assembly.GetManifestResourceStream(filePath) ?? throw new InvalidOperationException();
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd(); 
    }
}