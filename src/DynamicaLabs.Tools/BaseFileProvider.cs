using System.IO;

namespace DynamicaLabs.Tools
{
    public abstract class BaseFileProvider : IFileProvider
    {
        public abstract Stream GetContent(string path);
        public abstract void SetContent(string path, Stream value);

        public virtual string GetContentString(string path)
        {
            using (var reader = new StreamReader(GetContent(path)))
            {
                return reader.ReadToEnd();
            }
        }

        public virtual void SetContent(string path, string value)
        {
            SetContent(path, GenerateStreamFromString(value));
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}