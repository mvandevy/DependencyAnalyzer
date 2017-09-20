namespace Analysis.Core.Parsing.FileReading
{
    internal interface IFileReader
    {
        string ReadFileContents(string filePath);
    }
}