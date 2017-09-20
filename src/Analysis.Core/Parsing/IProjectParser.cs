using Analysis.Core.Models;

namespace Analysis.Core.Parsing
{
    public interface IProjectParser
    {
        Project Parse(string projectGuid, string projectName, string absolutePath);
    }
}