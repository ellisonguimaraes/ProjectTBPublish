using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Services
{
    public interface IGitHubRequest
    {
        Task<List<Repository>> GetFiveOldByLanguageRepos(string username, string language);
        Task<List<Repository>> GetAllRepos(string username);
        Task<User> GetUser(string username);
    }
}