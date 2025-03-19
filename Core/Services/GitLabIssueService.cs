using Core.Common;
using Core.Interfaces;
using Core.Mappers;
using Core.Models;
using Core.Settings;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Core.Services
{
    public interface IGitLabIssueService : IGitIssueService { }

    public class GitLabIssueService : IGitLabIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly GitServiceConfiguration _gitServiceConfiguration;
        private readonly HttpClientHelper _httpClientHelper;
        private const string HTTP_CLIENT_NAME = "GitLabHttpClient";


        public GitLabIssueService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _gitServiceConfiguration = new GitServiceConfiguration(configuration, GitServiceType.GitLab);

            _httpClient = httpClientFactory.CreateClient("HTTP_CLIENT_NAME");
            _httpClient.BaseAddress = new Uri(_gitServiceConfiguration.ApiBaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GitIssuesManager");
            _httpClient.DefaultRequestHeaders.Add("PRIVATE-TOKEN", _gitServiceConfiguration.ApiToken);

            _httpClientHelper = new HttpClientHelper(_httpClient);
        }

        public async Task<Issue> CreateIssueAsync(string repositoryOwner, string repositoryName, string title, string description)
        {
            string projectId = Uri.EscapeDataString($"{repositoryOwner}/{repositoryName}");

            var enpoint = $"/api/v4/projects/{projectId}/issues?title={title}&description={description}";
            var response = await _httpClientHelper.SendRequestAsync<GitLabIssueResponse>(enpoint, string.Empty, HttpMethod.Post);

            return IssueMapper.MapToIssue(response, repositoryOwner, repositoryName);
        }

        public async Task<Issue> UpdateIssueAsync(string repositoryOwner, string repositoryName, string issueId, string title, string description)
        {
            string projectId = Uri.EscapeDataString($"{repositoryOwner}/{repositoryName}");

            var requestBody = new
            {
                title,
                description
            };

            var enpoint = $"/api/v4/projects/{projectId}/issues/{issueId}";
            var response = await _httpClientHelper.SendRequestAsync<GitLabIssueResponse>(enpoint, requestBody, HttpMethod.Put);

            return IssueMapper.MapToIssue(response, repositoryOwner, repositoryName);
        }

        public async Task<Issue> CloseIssueAsync(string repositoryOwner, string repositoryName, string issueId)
        {
            string projectId = Uri.EscapeDataString($"{repositoryOwner}/{repositoryName}");

            var enpoint = $"/api/v4/projects/{projectId}/issues/{issueId}?state_event=close";
            var response = await _httpClientHelper.SendRequestAsync<GitLabIssueResponse>(enpoint, string.Empty, HttpMethod.Put);

            return IssueMapper.MapToIssue(response, repositoryOwner, repositoryName);
        }
    }
}