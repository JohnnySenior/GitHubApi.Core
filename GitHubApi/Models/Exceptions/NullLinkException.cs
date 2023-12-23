using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class NullLinkException : Xeption
    {
        public NullLinkException()
            : base(message: "Link is null.")
        { }
    }
}
