using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class GitHubRequestImplementation : IGitHubRequest
    {
        private const string _gitHubUrl = @"https://api.github.com/orgs/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<GitHubRequestImplementation> _logger;

        public GitHubRequestImplementation(HttpClient httpClient, ILogger<GitHubRequestImplementation> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Repository>> GetAllRepos(string username)
        {
            try {
                string url = _gitHubUrl + username + @"/repos" + "?page=1&per_page=120";
                
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ApiChallengeTakeBlip", "1"));

                var response = await _httpClient.GetAsync(url);

                _logger.LogInformation("STATUS CODE: " + response.StatusCode);
                _logger.LogInformation("LINK: " + url);

                // var teste = response.Content.ReadAsStringAsync().Result;
                // System.Console.WriteLine(teste);

                var allRepos = await JsonSerializer.DeserializeAsync<List<Repository>>(response.Content.ReadAsStream());
                _logger.LogInformation("QTD REPOS: " + allRepos.Count);
                return allRepos.OrderBy(re => re.CreatedAt).ToList();

            } catch (Exception) {
                _logger.LogInformation("Erro ao realizar a requisição");
                return null;
            }
        }

        public async Task<List<Repository>> GetFiveOldByLanguageRepos(string username, string language)
        {
            List<Repository> allRepositories = await GetAllRepos(username);

            if (allRepositories == null) return null;

            List<Repository> OldRepositoriesByLanguage = 
                    allRepositories
                        .Where(re => language.ToLower().Equals(re.Language?.ToLower()))
                        .ToList();

            return (OldRepositoriesByLanguage?.Count >= 5)? 
                        OldRepositoriesByLanguage.Skip(0).Take(5).ToList() 
                        : OldRepositoriesByLanguage;
        }

        public async Task<User> GetUser(string username)
        {
            try {
                string url = _gitHubUrl + username;
                
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ApiChallengeTakeBlip", "1"));

                var response = await _httpClient.GetAsync(url);

                _logger.LogInformation("STATUS CODE: " + response.StatusCode);
                _logger.LogInformation("LINK: " + url);

                var user = await JsonSerializer.DeserializeAsync<User>(response.Content.ReadAsStream());
                return user;

            } catch (Exception) {
                _logger.LogInformation("Erro ao realizar a requisição");
                return null;
            }
        }
    }
}