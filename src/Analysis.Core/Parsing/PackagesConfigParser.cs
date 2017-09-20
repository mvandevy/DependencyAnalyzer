using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Analysis.Core.Models;
using Analysis.Core.Parsing.FileReading;
using Serilog;

namespace Analysis.Core.Parsing
{
    internal class PackagesConfigParser : PackagesParser
    {
        public PackagesConfigParser(ILogger logger, IFileReader fileReader) 
            : base(logger,fileReader)
        {
        }

        public override IReadOnlyList<NugetPackage> Parse(string packagesConfigFilePath)
        {
            if (string.IsNullOrWhiteSpace(packagesConfigFilePath))
            {
                Logger.Warning($"packages.config file could not be found at the path: {packagesConfigFilePath}");
                return EmptyNugetPackagesList();
            }

            var contents = FileReader.ReadFileContents(packagesConfigFilePath);
            if (contents == string.Empty)
            {
                Logger.Warning($"Config file located at {packagesConfigFilePath} is empty");
                return EmptyNugetPackagesList();
            }

            XDocument xDocument;
            try
            {
                xDocument = XDocument.Parse(contents);
            }
            catch (XmlException e)
            {
                Logger.Warning($"Config file located at {packagesConfigFilePath} contains invalid XML - {e.Message}");
                return EmptyNugetPackagesList();
            }

            try
            {
                var packages = from package in xDocument.Descendants("package")
                    select new NugetPackage(
                        package?.Attribute("id")?.Value,
                        package?.Attribute("version")?.Value);

                return packages.ToList();
            }
            catch (NullReferenceException)
            {
                Logger.Warning($"Config file located at {packagesConfigFilePath} contains package elements with missing id or version attributes");
                return EmptyNugetPackagesList();
            }
        }
    }
}