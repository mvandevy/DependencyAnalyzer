using System.Collections.Generic;
using System.Linq;
using Analysis.Core.Models;
using Analysis.Nuget.Models;

namespace Analysis.Nuget.Grouping
{
    public class PackageReferenceProjectsGrouper
    {
        public IReadOnlyList<PackageReferenceProjectsGrouping> Group(IReadOnlyList<Project> projects)
        {
            List<PackageReferenceProjectsGrouping> returnValue;

            if (projects == null)
            {
                returnValue = new List<PackageReferenceProjectsGrouping>();
            }
            else
            {
                IEnumerable<(Project project, NugetPackage nuget)> nugets =
                    projects.SelectMany(project => project.NugetPackages.Select(nuget => (project, nuget)));

                var nugetsGroupedByPackage = nugets.GroupBy(nugetAndProject => nugetAndProject.nuget);

                var groups = new List<PackageReferenceProjectsGrouping>();
                foreach (var grouping in nugetsGroupedByPackage)
                {
                    var group = new PackageReferenceProjectsGrouping(grouping.Key.Name, grouping.Key.Version);
                    foreach (var projectNugeTuple in grouping)
                    {
                        group.ReferencedBy(new PackageProject(projectNugeTuple.project.Name, projectNugeTuple.project.Guid));
                    }
                    groups.Add(group);
                }

                returnValue = groups;
            }

            return returnValue;
        }
    }
}