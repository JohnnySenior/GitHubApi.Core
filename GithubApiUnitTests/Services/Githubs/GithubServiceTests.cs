using System.Linq.Expressions;
using GitHubApi.Brokers.Github;
using GitHubApi.Brokers.Loggings;
using GitHubApi.Services.Github;
using Moq;
using Octokit;
using Tynamix.ObjectFiller;
using Xeptions;

namespace GithubApiUnitTests.Services.Githubs
{
    public partial class GithubServiceTests
    {
        private readonly Mock<IGithubBroker> githubBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IGithubService githubService;

        public GithubServiceTests()
        {
            this.githubBrokerMock = new Mock<IGithubBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.githubService = new GithubService(
                githubBroker: this.githubBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private static string CreateRandomRepositoryLink()
        {
            var owner = new MnemonicString().GetValue();
            var repo = new MnemonicString().GetValue();

            return $"https://github.com/{owner}/{repo}";
        }

        private static WorkflowJobStep CreateRandomStep() =>
            CreateFillerStep().Create();

        private static Filler<WorkflowJobStep> CreateFillerStep()
        {
            var filler = new Filler<WorkflowJobStep>();

            filler.Setup()
               .OnProperty(step => step.Name).Use(() => "RandomStep")
               .OnProperty(step => step.StartedAt).Use(() => DateTimeOffset.Now)
               .OnProperty(step => step.CompletedAt).Use(() => DateTimeOffset.Now.AddSeconds(5))
               .OnProperty(step => step.Conclusion).Use(() => "Success")
               .OnProperty(step => step.Status).Use(() => WorkflowJobStatus.Completed);

            return filler;
        }

        private static List<WorkflowJobStep> CreateRandomSteps()
        {
            return CreateFillerStep().Create(count: 8).ToList();
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
