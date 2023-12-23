using GitHubApi.Models.Exceptions;
using GitHubApi.Services.Github;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using RESTFulSense.Controllers;

namespace GitHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GithubController : RESTFulController
    {
        private readonly IGithubService githubService;

        public GithubController(IGithubService githubService)
        {
            this.githubService = githubService;
        }

        [HttpGet]
        public async ValueTask<ActionResult<string>> GetRepoAnalysis(string repositoryLink)
        {
            try
            {
                string repoAnalysis =
                await this.githubService.GenerateRepoAnalysisAsync(repositoryLink);

                return Ok(repoAnalysis);
            }
            catch(LinkValidationException linkValidationException)
            {
                return BadRequest(linkValidationException.InnerException);
            }
            catch (RepoAnalysisValidationException repoAnalysisValidationException)
            {
                return InternalServerError(repoAnalysisValidationException.InnerException);
            }
            catch(RepoAnalysisServiceException repoAnalysisServiceException)
            {
                return InternalServerError(repoAnalysisServiceException.InnerException);
            }
        }
    }
}
