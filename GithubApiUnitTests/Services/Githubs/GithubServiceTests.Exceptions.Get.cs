using FluentAssertions;
using GitHubApi.Models.Exceptions;
using Moq;
using Octokit;
using Xunit;

namespace GithubApiUnitTests.Services.Githubs
{
    public partial class GithubServiceTests
    {
        [Fact]
        public async Task ShouldThrowOctokitNotFoundExceptionOnGetRepoAnalysisIfWorkflowIsNotFoundAndLogItAsync()
        {
            // given
            string someLink = CreateRandomRepositoryLink();

            var apiException = new ApiException();
            var notFoundWorkflowException = new NotFoundWorkflowException(apiException);

            var expectedRepoAnalysisValidationException =
                new RepoAnalysisValidationException(notFoundWorkflowException);

            this.githubBrokerMock.Setup(broker =>
                broker.GetLatestActionResultByRepositoryLink(someLink)).ThrowsAsync(apiException);

            // when
            ValueTask<string> getLinkTask = this.githubService.GenerateRepoAnalysisAsync(someLink);

            RepoAnalysisValidationException actualRepoAnalysisValidationException =
                await Assert.ThrowsAsync<RepoAnalysisValidationException>(getLinkTask.AsTask);

            // then
            actualRepoAnalysisValidationException.Should()
                .BeEquivalentTo(expectedRepoAnalysisValidationException);

            this.githubBrokerMock.Verify(broker =>
                broker.GetLatestActionResultByRepositoryLink(someLink), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRepoAnalysisValidationException))), Times.Once);

            this.githubBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetRepoAnalysisIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someLink = CreateRandomRepositoryLink();

            var serviceException = new Exception();

            var failedRepoAnalysisServiceException =
                new FailedRepoAnalysisServiceException(serviceException);

            var expectedRepoAnalysisServiceException =
                new RepoAnalysisServiceException(failedRepoAnalysisServiceException);

            this.githubBrokerMock.Setup(broker =>
                broker.GetLatestActionResultByRepositoryLink(someLink)).ThrowsAsync(serviceException);

            // when
            ValueTask<string> getLinkTask = this.githubService.GenerateRepoAnalysisAsync(someLink);

            RepoAnalysisServiceException actualRepoAnalysisServiceException =
                await Assert.ThrowsAsync<RepoAnalysisServiceException>(getLinkTask.AsTask);

            // then
            actualRepoAnalysisServiceException.Should()
                .BeEquivalentTo(expectedRepoAnalysisServiceException);

            this.githubBrokerMock.Verify(broker =>
                broker.GetLatestActionResultByRepositoryLink(someLink), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRepoAnalysisServiceException))), Times.Once);

            this.githubBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
