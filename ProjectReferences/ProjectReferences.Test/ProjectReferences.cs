using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace ProjectReferencesTest;

public class ProjectReferences
{
    private const string SLN_PATH = @"../../../../ProjectReferences.sln";
    private readonly Dictionary<string, Project> _solutionProjects;

    public ProjectReferences()
    {
        MSBuildLocator.RegisterDefaults();
        var msWorkspace = MSBuildWorkspace.Create();
        var solution = msWorkspace.OpenSolutionAsync(SLN_PATH).Result;
        _solutionProjects = new Dictionary<string, Project>();
        foreach (var project in solution.Projects)
        {
            _solutionProjects.Add(project.Name, project);
        }
    }

    [TestCase("API", new[] { "Business" })]
    [TestCase("Business", new[] { "Core", "Infrastructure" })]
    [TestCase("Infrastructure", new[] { "Core" })]
    [TestCase("Core", new string[] {})]
    public void CheckProjectReferenceRules(string projectName, string[] permittedReferences)
    {
        //Arrange
        var sut = _solutionProjects[projectName];
        var permittedProjects = permittedReferences.Select(pr => _solutionProjects[pr]).ToList();
        
        //Assert
        Assert.That(sut.ProjectReferences.Count(), Is.EqualTo(permittedReferences.Length));
        foreach (var reference in sut.ProjectReferences)
        {
            Assert.That(
                permittedProjects.Any(p => p.Id.Equals(reference.ProjectId)),
                Is.True
            );
        }
    }
}