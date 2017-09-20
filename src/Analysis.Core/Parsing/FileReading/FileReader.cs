using System.IO;

namespace Analysis.Core.Parsing.FileReading
{
    class FileReader : IFileReader
    {
        public string ReadFileContents(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}