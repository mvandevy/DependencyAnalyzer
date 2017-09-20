using Analysis.Core.Models;
using Analysis.Core.Parsing.FileReading;
using Serilog;
using System.Collections.Generic;

namespace Analysis.Core.Parsing
{
    internal abstract class PackagesParser
    {
        protected IFileReader FileReader { get; }
        protected ILogger Logger { get; }

        protected PackagesParser(ILogger logger, IFileReader fileReader)
        {
            Logger = logger;
            FileReader = fileReader;
        }

        public abstract IReadOnlyList<NugetPackage> Parse(string filePath);

        protected IReadOnlyList<NugetPackage> EmptyNugetPackagesList()
        {
            return new List<NugetPackage>();
        }
    }
}