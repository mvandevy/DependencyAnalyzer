using System.Collections.Generic;
using System.Linq;

namespace Analysis.Nuget.Comparing
{
    public class ProjectsPackageReferencesComparer
    {
        public IReadOnlyList<PackageReferenceDifferences> Compare(IEnumerable<Core.Models.Project> projects)
        {
            IEnumerable<(string projectName, Core.Models.NugetPackage nuget)> nugets =
                projects.SelectMany(project => project.NugetPackages.Select(nuget => (project.Name, nuget)));

            var nugetsGroupedByPackage = nugets.GroupBy(nugetAndProject => nugetAndProject.nuget.Name);

            var differences = new List<PackageReferenceDifferences>();
            foreach (var nugetsGrouping in nugetsGroupedByPackage)
            {
                if (nugetsGrouping.Count() == 1) continue;

                var versions = nugetsGrouping.GroupBy(nugetAndProject => nugetAndProject.nuget.Version).ToList();
                var versionDifferences = new List<VersionProjectsGrouping>();
                if (versions.Count > 1)
                {
                    versionDifferences = versions.Select(item => new VersionProjectsGrouping(item.Key, item.Select(i => i.projectName))).ToList();
                }

                if (versionDifferences.Count > 0)
                {
                    var packageDifferences = new PackageReferenceDifferences(nugetsGrouping.Key, versionDifferences);
                    differences.Add(packageDifferences);
                }
            }

            return differences;
        }
    }
}