using System.Collections.Generic;
using Analysis.Nuget.Models;

namespace Analysis.Nuget
{
    public class PackageAnalysisResult
    {
        public List<PackageReferenceProjectsGrouping> Packages { get; }

        public PackageAnalysisResult()
        {
            Packages = new List<PackageReferenceProjectsGrouping>();
        }

        //public void AddPackage
        // Solution exists out of the following projects
        // ...
        // Each project has the following package references

        // or

        // The following packages are referenced:
        // ...
        // Each package is being used in version x and is being referenced by:
        // - project x
        // - project y
    }
}