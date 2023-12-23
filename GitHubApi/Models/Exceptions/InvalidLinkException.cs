using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class InvalidLinkException : Xeption
    {
        public InvalidLinkException()
            : base(message: "Link is invalid.")
        { }
    }
}
