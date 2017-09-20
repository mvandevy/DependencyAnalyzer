using System.Collections.Generic;
using Analysis.Core.Models;

namespace Analysis.Core.Parsing
{
    public interface ISolutionParser
    {
        IReadOnlyList<Project> GetProjects();
    }
}