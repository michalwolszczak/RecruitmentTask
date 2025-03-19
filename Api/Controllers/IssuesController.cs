using Api.Models;
using Core.Factories;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/issues")]
    public class IssuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGitIssueServiceFactory _serviceFactory;

        public IssuesController(IConfiguration configuration, IGitIssueServiceFactory serviceFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add issue")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Issue), ContentTypes = ["application/json"])]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        public async Task<ActionResult<Issue>> CreateIssue([FromBody] CreateIssueRequest request)
        {
            var service = _serviceFactory.CreateService(request.ServiceType);

            var issue = await service.CreateIssueAsync(
                request.RepositoryOwner,
                request.RepositoryName,
                request.Title,
                request.Description);

            return Created($"api/issues/{issue.Id}", IssueResponse.FromIssue(issue, request.ServiceType));
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update issue")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Issue), ContentTypes = ["application/json"])]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        public async Task<ActionResult<IssueResponse>> UpdateIssue([FromBody] UpdateIssueRequest request)
        {
            var service = _serviceFactory.CreateService(request.ServiceType);

            var issue = await service.UpdateIssueAsync(
                request.RepositoryOwner,
                request.RepositoryName,
                request.IssueId,
                request.Title,
                request.Description);

            return Ok(IssueResponse.FromIssue(issue, request.ServiceType));
        }

        [HttpPost("close")]
        [SwaggerOperation(Summary = "Close issue")]
        [SwaggerResponse(StatusCodes.Status204NoContent, ContentTypes = ["application/json"])]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        public async Task<IActionResult> CloseIssue([FromBody] CloseIssueRequest request)
        {
            var service = _serviceFactory.CreateService(request.ServiceType);

            var issue = await service.CloseIssueAsync(
                request.RepositoryOwner,
                request.RepositoryName,
                request.IssueId);

            return NoContent();
        }
    }
}