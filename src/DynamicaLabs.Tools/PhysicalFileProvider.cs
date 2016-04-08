using System.IO;

namespace DynamicaLabs.Tools
{
    public sealed class PhysicalFileProvider : BaseFileProvider
    {
        public override Stream GetContent(string path)
        {
            return File.OpenRead(path);
        }

        public override void SetContent(string path, Stream value)
        {
            using (var writer = new StreamWriter(new FileStream(path, FileMode.Truncate)))
            using (var reader = new StreamReader(value))
            {
                writer.Write(reader.ReadToEnd());
            }
        }
    }
}