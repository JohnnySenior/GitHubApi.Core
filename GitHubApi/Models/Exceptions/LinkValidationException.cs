using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class LinkValidationException : Xeption
    {
        public LinkValidationException(Xeption innerException)
            : base(message: "Link validation error occurred, fix the error and try again.", innerException)
        { }
    }
}
