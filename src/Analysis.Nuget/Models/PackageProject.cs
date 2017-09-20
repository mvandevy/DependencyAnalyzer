namespace Analysis.Nuget.Models
{
    public class PackageProject
    {
        public string Name { get; }

        public string Guid { get; }

        public PackageProject(string name, string guid)
        {
            Name = name;
            Guid = guid;
        }
    }
}