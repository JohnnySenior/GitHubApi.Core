using FluentAssertions;
using Moq;
using Octokit;
using Xunit;

namespace GithubApiUnitTests.Services.Githubs
{
    public partial class GithubServiceTests
    {
        [Fact]
        public async Task ShouldGetRepoAnalysisAsync()
        {
            // given
            WorkflowJobStep rendomSteps = CreateRandomStep();

            var incomingRepositoryLink = CreateRandomRepositoryLink();
            var persistedJobSteps = CreateRandomSteps();
            var expectedRepoAnalysis = this.githubService.ParseToString(persistedJobSteps);

            githubBrokerMock.Setup(broker =>
                broker.GetLatestActionResultByRepositoryLink(incomingRepositoryLink))
                    .ReturnsAsync(persistedJobSteps);

            // when
            string actualRepoAnalysis =
                await githubService.GenerateRepoAnalysisAsync(incomingRepositoryLink);

            // then
            actualRepoAnalysis.Should().BeEquivalentTo(expectedRepoAnalysis);

            githubBrokerMock.Verify(broker =>
                broker.GetLatestActionResultByRepositoryLink(incomingRepositoryLink), Times.Once);

            githubBrokerMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
