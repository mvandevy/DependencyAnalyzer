using System.Linq;
using Analysis.Core.Parsing;
using Analysis.Nuget.Comparing;
using Analysis.Nuget.Grouping;
using Serilog;

namespace Analysis.Nuget
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            if (args.Length != 1)
            {
                logger.Information("Analysis.Nuget expects a single parameter, the path of the solution to analyse.");
                logger.Information("");
                logger.Information("Usage: Analysis.Nuget.exe <path of solution to analyse>");
                logger.Information("  E.g. Analysis.Nuget.exe \"C:\\repos\\sample.sln\"");
                logger.Information("");
                return;
            }

            var path = args[0];
            var projectParser = new ProjectParser(logger);
            var solutionParser = new SolutionParser(path, projectParser, logger);
            var projects = solutionParser.GetProjects();

            var packageProjectsGrouper = new PackageReferenceProjectsGrouper();
            var packageProjectsGrouping = packageProjectsGrouper.Group(projects);

            foreach (var grouping in packageProjectsGrouping)
            {
                logger.Information("{Package} {Version} is being referenced by"
                    , grouping.Name
                    , grouping.Version);
                //System.Console.WriteLine($"##vso[task.logissue type=warning;]{grouping.Name} {grouping.Version} is being referenced by");

                foreach (var project in grouping.Projects)
                {
                    logger.Information("{ProjectName}"
                        , project.Name);
                }
                logger.Information("");
            }

            var comparer = new ProjectsPackageReferencesComparer();
            var packageReferenceDifferences = comparer.Compare(projects);

            foreach (var difference in packageReferenceDifferences)
            {
                if (difference.VersionDifferences.Count > 0)
                {
                    logger.Information("{Package} has multiple versions {Versions} referenced in different projects"
                        , difference.PackageName
                        , difference.VersionDifferences.Select(versionDifference => versionDifference.Version));

                    foreach (var versionDifference in difference.VersionDifferences)
                    {
                        logger.Information("{Package} {Version} referenced in projects {Projects}",
                            difference.PackageName,
                            versionDifference.Version,
                            versionDifference.ProjectNames);
                    }
                }
            }
        }
    }
}