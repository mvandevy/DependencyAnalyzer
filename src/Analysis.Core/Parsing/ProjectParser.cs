using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Analysis.Core.Models;
using Analysis.Core.Parsing.FileReading;
using Serilog;

namespace Analysis.Core.Parsing
{
    public class ProjectParser : IProjectParser
    {
        private readonly ILogger _logger;

        public ProjectParser(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Project Parse(string projectGuid, string projectName, string absolutePath)
        {
            AssertInitialProperties(projectGuid, projectName, absolutePath);

            var nugetPackages = ExtractNugetPackages(absolutePath);

            return new Project(projectName, projectGuid, absolutePath, nugetPackages);
        }

        private void AssertInitialProperties(string projectGuid, string projectName, string absolutePath)
        {
            if (string.IsNullOrEmpty(projectGuid)) throw new ArgumentNullException(nameof(projectGuid));
            if (string.IsNullOrEmpty(projectName)) throw new ArgumentNullException(nameof(projectName));
            if (string.IsNullOrEmpty(absolutePath)) throw new ArgumentNullException(nameof(absolutePath));
        }

        private NugetFormatType DetermineNugetFormatType(string csprojAbsolutePath)
        {
            using (var reader = XmlReader.Create(csprojAbsolutePath))
            {
                var rootElement = XElement.Load(reader);
                var nameTable = reader.NameTable;
                if (nameTable != null)
                {
                    var namespaceManager = new XmlNamespaceManager(nameTable);
                    namespaceManager.AddNamespace("msbuild", Constants.MSBUILD_NAMESPACE);

                    // look for /ItemGroup/PackageReference elements
                    var packageReferenceItemGroups = rootElement.XPathSelectElements("./msbuild:ItemGroup/msbuild:PackageReference", namespaceManager);
                    if (packageReferenceItemGroups.Any()) return NugetFormatType.UsedPackageReferenceElements;

                    var packageConfigElements = rootElement.XPathSelectElements("./msbuild:ItemGroup/msbuild:None[@Include='packages.config']", namespaceManager);
                    if (packageConfigElements.Any()) return NugetFormatType.UsesPackagesConfigFile;
                }
            }

            return NugetFormatType.Undetermined;
        }

        private IReadOnlyList<NugetPackage> ExtractNugetPackages(string absoluteProjectPath)
        {
            var nugetFormatType = DetermineNugetFormatType(absoluteProjectPath);

            IReadOnlyList<NugetPackage> result = new List<NugetPackage>();
            switch (nugetFormatType)
            {
                case NugetFormatType.UsesPackagesConfigFile:
                    var configParser = new PackagesConfigParser(_logger, new FileReader());
                    var path = BuildPackagesConfigPath(absoluteProjectPath);
                    result = configParser.Parse(path);
                    break;
                case NugetFormatType.UsedPackageReferenceElements:
                    var csprojParser = new PackagesCsprojParser(_logger, new FileReader());
                    result = csprojParser.Parse(absoluteProjectPath);
                    break;
            }

            return result;
        }

        private string BuildPackagesConfigPath(string absoluteProjectPath)
        {
            if (string.IsNullOrWhiteSpace(absoluteProjectPath)) throw new ArgumentNullException(nameof(absoluteProjectPath));
            if (!Path.IsPathRooted(absoluteProjectPath)) throw new ArgumentException($"Path: \"{absoluteProjectPath}\" is not rooted.", nameof(absoluteProjectPath));
            var dirName = Path.GetDirectoryName(absoluteProjectPath);
            if (dirName == null) throw new ArgumentException($"{absoluteProjectPath} does not contain a path", nameof(dirName));
            return Path.Combine(dirName, "packages.config");
        }
    }
}