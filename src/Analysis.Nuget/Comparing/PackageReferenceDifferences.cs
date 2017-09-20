using System;
using System.Collections.Generic;
using System.Linq;

namespace Analysis.Nuget.Comparing
{
    public class PackageReferenceDifferences : IEquatable<PackageReferenceDifferences>
    {
        public PackageReferenceDifferences(
            string packageReferenceName,
            IReadOnlyList<VersionProjectsGrouping> versionDifferences)
        {
            PackageName = packageReferenceName;
            VersionDifferences = versionDifferences;
        }

        public string PackageName { get; }

        public IReadOnlyList<VersionProjectsGrouping> VersionDifferences { get; }

        public bool Equals(PackageReferenceDifferences other)
        {
            if (other == null) return false;

            return PackageName == other.PackageName &&
                   VersionDifferences.SequenceEqual(other.VersionDifferences);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != this.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals((PackageReferenceDifferences) obj);
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(PackageReferenceDifferences)} is not intended to be the key in a collection.");
        }

        public override string ToString()
        {
            return PackageName;
        }
    }
}