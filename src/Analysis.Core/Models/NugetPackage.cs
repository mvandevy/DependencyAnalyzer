using System;

namespace Analysis.Core.Models
{
    public class NugetPackage : IEquatable<NugetPackage>
    {
        public string Name { get; }
        public string Version { get; }

        public NugetPackage(string name, string version)
        {
            Name = name;
            Version = version;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != GetType())
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals((NugetPackage)obj);
        }

        public bool Equals(NugetPackage other)
        {
            if (other == null) return false;

            return Name == other.Name &&
                Version == other.Version;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Version != null ? Version.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Name}, {Version}";
        }
    }
}