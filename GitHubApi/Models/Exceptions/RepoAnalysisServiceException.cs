using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class RepoAnalysisServiceException : Xeption
    {
        public RepoAnalysisServiceException(Xeption innerException)
            : base(message: "Repository analysis service error occurred, contact support.", innerException)
        { }
    }
}
