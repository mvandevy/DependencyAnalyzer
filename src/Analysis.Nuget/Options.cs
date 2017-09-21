using System.Text;
using CommandLine;

namespace Analysis.Nuget
{
    internal class Options
    {
        [Option('s', "solution", Required = true, HelpText = "Path of the solution file to process.")]
        public string SolutionFilePath { get; set; }

        [Option('f', "format", Required = false, DefaultValue = "vsts", HelpText = "Style in which the output needs to be written to the console.")]
        public string OutputFormat { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("Analysis.Nuget expects a single parameter, the path of the solution to analyse.");
            usage.AppendLine("");
            usage.AppendLine("Usage: Analysis.Nuget.exe -s <path of solution to analyse>");
            usage.AppendLine("  E.g. Analysis.Nuget.exe -s \"C:\\repos\\sample.sln\"");
            return usage.ToString();
        }
    }
}