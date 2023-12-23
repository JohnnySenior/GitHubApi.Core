using System.Text;
using GitHubApi.Brokers.Github;
using GitHubApi.Brokers.Loggings;
using Octokit;

namespace GitHubApi.Services.Github
{
    public partial class GithubService : IGithubService
    {
        private readonly IGithubBroker githubBroker;
        private readonly ILoggingBroker loggingBroker;

        public GithubService(IGithubBroker githubBroker, ILoggingBroker loggingBroker)
        {
            this.githubBroker = githubBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<string> GenerateRepoAnalysisAsync(string repositoryLink) =>
        TryCatch(async () =>
        {
            ValidateLinkOnGet(repositoryLink);

            List<WorkflowJobStep> jobSteps = await this.githubBroker
                .GetLatestActionResultByRepositoryLink(repositoryLink);

            string repoAnalysis = ParseToString(jobSteps);

            return repoAnalysis;
        });

        public string ParseToString(List<WorkflowJobStep> stepsList)
        {
            var stringBuilder = new StringBuilder();

            foreach (var step in stepsList)
            {
                stringBuilder.AppendLine($"Task Name: {step.Name}, Started: {step.StartedAt.Value.TimeOfDay} " +
                    $"Complated: {step.CompletedAt.Value.TimeOfDay} Task Result: {step.Conclusion}");
            }

            return stringBuilder.ToString();
        }
    }
}
