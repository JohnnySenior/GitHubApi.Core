using FluentAssertions;
using GitHubApi.Models.Exceptions;
using Moq;
using Xunit;

namespace GithubApiUnitTests.Services.Githubs
{
    public partial class GithubServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnGetRepoAnalysisIfLinkIsNullAndLogAsync()
        {
            // given
            string nullRepositoryLink = null;

            var nullLinkException = new NullLinkException();

            var expectedLinkValidationException =
                new LinkValidationException(nullLinkException);

            // when
            ValueTask<string> getLinkTask =
                this.githubService.GenerateRepoAnalysisAsync(nullRepositoryLink);

            LinkValidationException actualLinkValidationException =
                await Assert.ThrowsAsync<LinkValidationException>(getLinkTask.AsTask);

            // then
            actualLinkValidationException.Should()
                .BeEquivalentTo(expectedLinkValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLinkValidationException))), Times.Once);

            this.githubBrokerMock.Verify(broker =>
                broker.GetLatestActionResultByRepositoryLink(It.IsAny<string>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.githubBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalidLink")]
        [InlineData("https://github.com")]
        [InlineData("https://github.com/owner")]
        public async Task ShouldThrowValidationExceptionOnGetRepoAnalysisIfLinkIsInvalidAndLogAsync(string invalidRepositoryLink)
        {
            // given
            string invalidLink = invalidRepositoryLink;

            var invalidLinkException = new InvalidLinkException();

            invalidLinkException.AddData(
                key: nameof(invalidLink),
                values: "You should provide a valid link in the format 'https://github.com/owner/repo'.");

            var expectedLinkValidationException = new LinkValidationException(invalidLinkException);

            // when
            ValueTask<string> getLinkTask = this.githubService.GenerateRepoAnalysisAsync(invalidLink);

            LinkValidationException actualLinkValidationException =
                await Assert.ThrowsAsync<LinkValidationException>(getLinkTask.AsTask);

            // then
            actualLinkValidationException.Should().BeEquivalentTo(expectedLinkValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLinkValidationException))), Times.Once);

            this.githubBrokerMock.Verify(broker =>
                broker.GetLatestActionResultByRepositoryLink(It.IsAny<string>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.githubBrokerMock.VerifyNoOtherCalls();
        }
    }
}
