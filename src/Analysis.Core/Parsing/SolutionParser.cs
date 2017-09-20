using System;
using System.Collections.Generic;
using Microsoft.Build.Construction;
using Serilog;
using Project = Analysis.Core.Models.Project;

namespace Analysis.Core.Parsing
{
    public class SolutionParser : ISolutionParser
    {
        private readonly ILogger _logger;
        private readonly string _solutionFilePath = string.Empty;
        private readonly IProjectParser _projectParser;

        public SolutionParser(string solutionFilePath, IProjectParser parser, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (string.IsNullOrWhiteSpace(solutionFilePath)) return;
            _solutionFilePath = solutionFilePath;
            _projectParser = parser;
        }

        public IReadOnlyList<Project> GetProjects()
        {
            var solutionFile = SolutionFile.Parse(_solutionFilePath);
            var projectsInSolution = solutionFile.ProjectsInOrder;
            var projects = new List<Project>();
            foreach (var projectInSolution in projectsInSolution)
            {
                var project =_projectParser.Parse(projectInSolution.ProjectGuid, projectInSolution.ProjectName, projectInSolution.AbsolutePath);
                projects.Add(project);
            }
            return projects;
        }
    }
}