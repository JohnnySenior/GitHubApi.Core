using Xeptions;

namespace GitHubApi.Models.Exceptions
{
    public class NotFoundWorkflowException : Xeption
    {
        public NotFoundWorkflowException(Exception innerException)
            : base(message: "Workflow file not found", innerException)
        { }
    }
}
