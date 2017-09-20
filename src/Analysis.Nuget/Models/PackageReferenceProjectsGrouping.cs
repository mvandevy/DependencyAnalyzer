using System;
using System.Collections.Generic;
using System.Linq;

namespace Analysis.Nuget.Models
{
    public class PackageReferenceProjectsGrouping : IEquatable<PackageReferenceProjectsGrouping>
    {
        public string Name { get; }
        public string Version { get; }

        private readonly List<PackageProject> _projects;
        public IReadOnlyList<PackageProject> Projects => _projects;

        public PackageReferenceProjectsGrouping(string name, string version)
        {
            Name = name;
            Version = version;
            _projects = new List<PackageProject>();
        }

        public void ReferencedBy(PackageProject packageProject)
        {
            if (_projects != null && packageProject != null)
            {
                if (_projects.Any(p => p.Guid?.ToLowerInvariant() == packageProject.Guid?.ToLowerInvariant())) return;
                _projects.Add(packageProject);
            }
        }

        public bool Equals(PackageReferenceProjectsGrouping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Version, other.Version) && Equals(Projects, other.Projects);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PackageReferenceProjectsGrouping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Version != null ? Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Projects != null ? Projects.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}