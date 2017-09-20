using System;
using System.Collections.Generic;

namespace Analysis.Core.Models
{
    public class Project : IEquatable<Project>
    {
        public string Name { get; }
        public string Guid { get; }
        public string AbsolutePath { get; }
        public IReadOnlyList<NugetPackage> NugetPackages { get; }

        public Project(string name, string guid, string absolutePath, IReadOnlyList<NugetPackage> nugetPackages)
        {
            Name = name;
            Guid = guid;
            AbsolutePath = absolutePath;
            NugetPackages = nugetPackages;
        }

        public bool Equals(Project other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Guid, other.Guid) && string.Equals(AbsolutePath, other.AbsolutePath) && Equals(NugetPackages, other.NugetPackages);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Project) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Guid != null ? Guid.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AbsolutePath != null ? AbsolutePath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NugetPackages != null ? NugetPackages.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}