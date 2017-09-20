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
    internal class PackagesCsprojParser : PackagesParser
    {
        public PackagesCsprojParser(ILogger logger, IFileReader fileReader)
            : base(logger, fileReader)
        {
        }

        public override IReadOnlyList<NugetPackage> Parse(string csprojFilePath)
        {
            if (string.IsNullOrWhiteSpace(csprojFilePath))
            {
                Logger.Warning($"The .csproj file could not be found at the path: {csprojFilePath}");
                return EmptyNugetPackagesList();
            }

            var contents = FileReader.ReadFileContents(csprojFilePath);
            if (contents == string.Empty)
            {
                Logger.Warning($".csproj file located at {csprojFilePath} is empty");
                return EmptyNugetPackagesList();
            }

            XDocument xDocument;
            try
            {
                xDocument = XDocument.Parse(contents);
            }
            catch (XmlException e)
            {
                Logger.Warning($".csproj file located at {csprojFilePath} contains invalid XML - {e.Message}");
                return EmptyNugetPackagesList();
            }

            try
            {
                var itemGroupWithPackageReferences = xDocument.Element(XName.Get($"{{{Constants.MSBUILD_NAMESPACE}}}Project"))
                    ?
                    .Elements($"{{{Constants.MSBUILD_NAMESPACE}}}ItemGroup")
                    .Where(itemgroup => itemgroup.HasElements && (itemgroup.Elements($"{{{Constants.MSBUILD_NAMESPACE}}}PackageReference").Any()));

                var packageReferences = itemGroupWithPackageReferences?.Elements();

                var packages = from package in packageReferences?.ToList()
                               select new NugetPackage(
                                   package?.Attribute("Include")?.Value,
                                   package?.Element($"{{{Constants.MSBUILD_NAMESPACE}}}Version")?.Value);

                return packages.ToList();
            }
            catch (NullReferenceException)
            {
                Logger.Warning($".csproj file located at {csprojFilePath} contains package elements with missing id or version attributes");
                return EmptyNugetPackagesList();
            }
        }
    }
}