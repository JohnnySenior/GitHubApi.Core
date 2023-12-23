using System.Net;
using Octokit;

namespace GitHubApi.Brokers.Github
{
    public class GithubBroker : IGithubBroker
    {
        private readonly GitHubClient gitHubClient;

        public GithubBroker(IConfiguration configuration)
        {
            string accessToken = configuration[key: "GitHub:AccessToken"];

            this.gitHubClient = new GitHubClient(productInformation: new ProductHeaderValue("GitHubAccessApi"))
            {
                Credentials = new Credentials(token: accessToken)
            };
        }
        public async ValueTask<List<WorkflowJobStep>> GetLatestActionResultByRepositoryLink(string repositoryLink)
        {
            (string owner, string repo, string workflowName, string branch) = ParseRepositoryLink(repositoryLink);

            WorkflowRunsResponse runs = await this.gitHubClient.Actions.Workflows.Runs.ListByWorkflow(owner, repo, workflowName);

            if(runs is null)
            {
                throw new ArgumentNullException(nameof(runs));
            }

            WorkflowRun? latestRun = runs.WorkflowRuns.FirstOrDefault();

            long runId = latestRun.Id;

            WorkflowJobsResponse jobs = await this.gitHubClient.Actions.Workflows.Jobs.List(owner, repo, runId);

            WorkflowJob job = jobs.Jobs[0];

            List<WorkflowJobStep> jobStep = job.Steps.ToList();

            return jobStep;
        }

        private (string? owner, string? repo, string? workflowName, string? branch) ParseRepositoryLink(string repositoryLink)
        {
            repositoryLink = WebUtility.UrlDecode(encodedValue: repositoryLink);

            string[] segments = repositoryLink.Split(separator: '/', options: StringSplitOptions.RemoveEmptyEntries);

            string owner = segments[2];
            string repo = segments[3];
            string? branch = segments.Length > 4 ? segments[4] : null;

            string workflowName = "dotnet.yml";

            return (owner, repo, workflowName, branch);
        }
    }
}
