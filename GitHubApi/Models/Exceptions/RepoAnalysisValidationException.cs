using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class RepoAnalysisValidationException : Xeption
    {
        public RepoAnalysisValidationException(Xeption innerException)
            : base(message: "Repository analysis validation error occurred, contact support", innerException)
        { }
    }
}
