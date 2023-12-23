using Octokit;

namespace GitHubApi.Brokers.Github
{
    public interface IGithubBroker
    {
        ValueTask<List<WorkflowJobStep>> GetLatestActionResultByRepositoryLink(string repositoryLink);
    }
}
