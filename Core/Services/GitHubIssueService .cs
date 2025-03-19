using Core.Models;
using System.Net.Http.Headers;
using Core.Settings;
using Microsoft.Extensions.Configuration;
using Core.Interfaces;
using Core.Mappers;
using Core.Common;

namespace Core.Services
{
    public interface IGitHubIssueService : IGitIssueService { }

    public class GitHubIssueService : IGitHubIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly GitServiceConfiguration _gitServiceConfiguration;
        private readonly HttpClientHelper _httpClientHelper;
        private const string HTTP_CLIENT_NAME = "GitHubHttpClient";

        public GitHubIssueService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _gitServiceConfiguration = new GitServiceConfiguration(configuration, GitServiceType.GitHub);

            _httpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
            _httpClient.BaseAddress = new Uri(_gitServiceConfiguration.ApiBaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GitIssuesManager");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _gitServiceConfiguration.ApiToken);

            _httpClientHelper = new HttpClientHelper(_httpClient);
        }

        public async Task<Issue> CreateIssueAsync(string repositoryOwner, string repositoryName, string title, string description)
        {
            var requestBody = new
            {
                title,
                body = description
            };

            var enpoint = $"/repos/{repositoryOwner}/{repositoryName}/issues";

            var response = await _httpClientHelper.SendRequestAsync<GitHubIssueResponse>(enpoint, requestBody, HttpMethod.Post);

            return IssueMapper.MapToIssue(response, repositoryOwner, repositoryName);
        }

        public async Task<Issue> UpdateIssueAsync(string repositoryOwner, string repositoryName, string issueId, string title, string description)
        {
            var requestBody = new
            {
                title,
                body = description
            };

            var enpoint = $"/repos/{repositoryOwner}/{repositoryName}/issues/{issueId}";

            var response = await _httpClientHelper.SendRequestAsync<GitHubIssueResponse>(enpoint, requestBody, HttpMethod.Patch);

            return IssueMapper.MapToIssue(response, repositoryOwner, repositoryName);
        }

        public async Task<Issue> CloseIssueAsync(string repositoryOwner, string repositoryName, string issueId)
        {
            var requestBody = new
            {
                state = "closed"
            };

            var enpoint = $"/repos/{repositoryOwner}/{repositoryName}/issues/{issueId}";

            var response = await _httpClientHelper.SendRequestAsync<GitHubIssueResponse>(enpoint, requestBody, HttpMethod.Patch);

            return IssueMapper.MapToIssue(response, repositoryOwner, repositoryName);
        }
    }
}