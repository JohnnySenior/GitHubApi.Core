using Octokit;
using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class InvalidRepoAnalysisException : Xeption
    {
        public InvalidRepoAnalysisException()
            : base(message: "Repository analysis is invalid, contact support")
        { }
    }
}
