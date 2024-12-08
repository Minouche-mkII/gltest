namespace gltest.Utils.IO;

public static class FileReader
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns>The file content</returns>
    /// <exception cref=""></exception>
    //TODO: documenter exceptions 
    public static string ReadAllText(string filename)
    {
        var streamReader = new StreamReader(filename);
        return streamReader.ReadToEnd();
    }
}