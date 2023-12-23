using System.Net;
using GitHubApi.Models.Exceptions;

namespace GitHubApi.Services.Github
{
    public partial class GithubService
    {
        private static void ValidateLinkOnGet(string invalidLink)
        {
            ValidateNotNullLink(invalidLink);

            Validate(
                (Rule: IsInvalidLink(invalidLink), Parameter: nameof(invalidLink)));
            Validate(
                (Rule: IsInvalidGitHubLink(invalidLink), Parameter: nameof(invalidLink)));

            Validate(
                (Rule: IsInvalidSegments(invalidLink), Parameter: nameof(invalidLink)));
        }

        private static void ValidateNotNullLink(string repositoryLink)
        {
            if (repositoryLink is null)
            {
                throw new NullLinkException();
            }
        }

        private static dynamic IsInvalidLink(string link)
        {
            string decodedLink = WebUtility.UrlDecode(link);

            return new
            {
                Condition = string.IsNullOrEmpty(link) || string.IsNullOrWhiteSpace(link),
                Message = "You should provide a valid link in the format 'https://github.com/owner/repo'."
            };
        }

        private static dynamic IsInvalidGitHubLink(string link)
        {
            string decodedLink = WebUtility.UrlDecode(link);

            return new
            {
                Condition = !link.StartsWith("https://github.com/"),
                Message = "You should provide a valid link in the format 'https://github.com/owner/repo'."
            };
        }

        private static dynamic IsInvalidSegments(string link)
        {
            string decodedLink = WebUtility.UrlDecode(link);

            return new
            {
                Condition = !Uri.TryCreate(new Uri("https://github.com/"), decodedLink, out var uri) || uri.AbsolutePath.Trim('/').Split('/').Length < 2,
                Message = "You should provide a valid link in the format 'https://github.com/owner/repo'."
            };
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLinkException = new InvalidLinkException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLinkException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLinkException.ThrowIfContainsErrors();
        }
    }
}
