using System;
using System.Collections.Generic;
using System.Linq;

namespace Analysis.Nuget.Comparing
{
    public class VersionProjectsGrouping : IEquatable<VersionProjectsGrouping>
    {
        public VersionProjectsGrouping(string version, IEnumerable<string> projectNames)
        {
            Version = version;
            ProjectNames = new List<string>(projectNames);
        }

        public string Version { get; }

        public IReadOnlyList<string> ProjectNames { get; }

        public bool Equals(VersionProjectsGrouping other)
        {
            if (other == null) return false;

            return Version == other.Version &&
                   ProjectNames.SequenceEqual(other.ProjectNames);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != GetType())
            {
                return false;
            }
            return ReferenceEquals(this, obj) || Equals((VersionProjectsGrouping)obj);
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(VersionProjectsGrouping)} is not intended to be the key in a collection.");
        }

        public override string ToString()
        {
            return Version;
        }
    }
}