using Octokit;

namespace GitHubApi.Services.Github
{
    public interface IGithubService
    {
        ValueTask<string> GenerateRepoAnalysisAsync(string repositoryLink);
        string ParseToString(List<WorkflowJobStep> stepsList);
    }
}
