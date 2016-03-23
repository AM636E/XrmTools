using System.IO;

namespace DynamicaLabs.Tools
{
    public interface IFileProvider
    {
        Stream GetContent(string path);
        string GetContentString(string path);

        void SetContent(string path, string value);
        void SetContent(string path, Stream value);
    }
}