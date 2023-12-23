using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class FailedRepoAnalysisServiceException : Xeption
    {
        public FailedRepoAnalysisServiceException(Exception innerException)
            : base(message: "Failed repository analysis service error occurred, contact support.", innerException)
        { }
    }
}
