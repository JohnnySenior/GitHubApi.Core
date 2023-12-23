using GitHubApi.Models.Exceptions;
using Octokit;
using Xeptions;

namespace GitHubApi.Services.Github
{
    public partial class GithubService
    {
        private delegate ValueTask<string> ReturningStringFunctions();

        private async ValueTask<string> TryCatch(ReturningStringFunctions returningStringFunctions)
        {
            try
            {
                return await returningStringFunctions();
            }
            catch (NullLinkException nullLinkException)
            {
                throw CreateAndLogValidationException(nullLinkException);
            }
            catch (InvalidLinkException invalidLinkException)
            {
                throw CreateAndLogValidationException(invalidLinkException);
            }
            catch (ApiException apiException)
            {
                var notFoundWorkflowException =
                    new NotFoundWorkflowException(apiException);

                throw CreateAndLogRepoAnalysisValidationException(notFoundWorkflowException);
            }
            catch (Exception exception)
            {
                var failedRepoAnalysisServiceException =
                    new FailedRepoAnalysisServiceException(exception);

                throw CreateAndLogServiceException(failedRepoAnalysisServiceException);
            }
        }

        private LinkValidationException CreateAndLogValidationException(Xeption exception)
        {
            var linkValidationException = new LinkValidationException(exception);
            this.loggingBroker.LogError(linkValidationException);

            return linkValidationException;
        }

        private RepoAnalysisValidationException CreateAndLogRepoAnalysisValidationException(Xeption exception)
        {
            var repoAnalysisValidationException = new RepoAnalysisValidationException(exception);
            this.loggingBroker.LogError(repoAnalysisValidationException);

            return repoAnalysisValidationException;
        }

        private RepoAnalysisServiceException CreateAndLogServiceException(Xeption exception)
        {
            var repoAnalysisServiceException = new RepoAnalysisServiceException(exception);
            this.loggingBroker.LogError(repoAnalysisServiceException);

            return repoAnalysisServiceException;
        }
    }
}
