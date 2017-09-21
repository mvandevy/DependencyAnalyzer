using Analysis.Core.Parsing;
using Analysis.Nuget.Comparing;
using Analysis.Nuget.Grouping;
using Serilog;
using Serilog.Core;
using System.Linq;

namespace Analysis.Nuget
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options)) return;
            Logger logger;
            if (options.OutputFormat?.Trim().ToLowerInvariant() == "vsts")
            {
                logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.VstsExtensionLog()
                    .CreateLogger();
            }
            else
            {
                logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger();
            }

            var path = options.SolutionFilePath;
            var projectParser = new ProjectParser(logger);
            var solutionParser = new SolutionParser(path, projectParser, logger);
            var projects = solutionParser.GetProjects();

            var packageProjectsGrouper = new PackageReferenceProjectsGrouper();
            var packageProjectsGrouping = packageProjectsGrouper.Group(projects);

            foreach (var grouping in packageProjectsGrouping)
            {
                logger.Information("{Package} {Version} is being referenced by the projects: {Projects}"
                    , grouping.Name
                    , grouping.Version
                    , grouping.Projects.Select(p => p.Name));
            }

            var comparer = new ProjectsPackageReferencesComparer();
            var packageReferenceDifferences = comparer.Compare(projects);

            foreach (var difference in packageReferenceDifferences)
            {
                if (difference.VersionDifferences.Count > 0)
                {
                    logger.Warning("{Package} has multiple versions {Versions} referenced in different projects"
                        , difference.PackageName
                        , difference.VersionDifferences.Select(versionDifference => versionDifference.Version));

                    foreach (var versionDifference in difference.VersionDifferences)
                    {
                        logger.Warning("{Package} {Version} referenced in projects {Projects}",
                            difference.PackageName,
                            versionDifference.Version,
                            versionDifference.ProjectNames);
                    }
                }
            }
        }
    }
}