# Test Project References in C#

This repository contains a dummy solution with no code. The purpose is to check if certain "referencing rules" are met using Unit Testing.

The solution contains 4 main projects with the following rules:
- API : Can only see Business
- Infrastructure : Can only see Core
- Core : Reference free
- Business : Can see Core and Infrastructure

## How can project references be tested?

There are several ways of testing that a project satisfies a given referencing rule set. In this case the selected option is through the use of Roslyn Analyzer, provided by Microsoft.

To be able to implement the testing code, the following NuGets are needed:
- Microsoft.Build.Locator
- Microsoft.CodeAnalysis.CSharp.Workspaces
- Microsoft.CodeAnalysis.Workspaces.Common
- Microsoft.CodeAnalysis.Workspaces.MSBuild

## Testing process

To be able to access the Solution from the code, the following snippet must be used:
```
MSBuildLocator.RegisterDefaults();
var msWorkspace = MSBuildWorkspace.Create();
var solution = msWorkspace.OpenSolutionAsync(SLN_PATH).Result;
```
After the execution of the previous code fragment, an instance of the solution will be available to use and test. From that instance, Projects and its References can be retrieved.

To define the project references ruleset, NUnit test cases will be used in this way:
```
[TestCase("API", new[] { "Business" })]
[TestCase("Business", new[] { "Core", "Infrastructure" })]
[TestCase("Infrastructure", new[] { "Core" })]
[TestCase("Core", new string[] {})]
```
