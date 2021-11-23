using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubRequest _githubRequest;
        public GitHubController(IGitHubRequest githubRequest)
        {
            _githubRequest = githubRequest;
        }
        
        /// <summary> Obtendo informações do Usuário </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Informações do usuário</returns>
        /// <response code="200">OK - Usuário retornado</response>
        /// <response code="400">BadRequest - Requisição invalidada</response>
        [HttpGet]
        [Route("{user}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromRoute]string user)
        {
            User getUser = await _githubRequest.GetUser(user);

            if (getUser == null) return BadRequest("Não foi possível obter informações do usuário");

            return Ok(getUser);
        }

        /// <summary> Obtendo todos os repositórios do usuário </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Repositórios do usuário</returns>
        /// <response code="200">OK - Repositórios retornados</response>
        /// <response code="400">BadRequest - Requisição invalidada</response>
        [HttpGet]
        [Route("{user}/repositories")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Repository>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRepositories([FromRoute]string user)
        {
            List<Repository> repositories = await _githubRequest.GetAllRepos(user);

            if(repositories == null) return BadRequest("Não foi possível obter os repositórios do usuário");

            return Ok(repositories);
        }

        /// <summary> Obtendo os 5 repositórios mais antigos do usuário, com base na linguagem do projeto </summary>
        /// <param name="user">Usuário</param>
        /// <param name="language">Linguagem do Projeto</param>
        /// <returns>Cinco repositórios mais antigos do usuário baseado na linguagem</returns>
        /// <response code="200">OK - Repositórios retornados</response>
        /// <response code="400">BadRequest - Requisição invalidada</response>
        [HttpGet]
        [Route("{user}/repositories/fiveoldbylanguage/{language}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFiveOldByLanguageRepositories([FromRoute]string user, [FromRoute]string language)
        {
            List<Repository> repositories = await _githubRequest.GetFiveOldByLanguageRepos(user, language);

            if(repositories == null) return BadRequest("Não foi possível obter os repositórios do usuário");

            return Ok(new {
                one = repositories[0],
                two = repositories[1],
                three = repositories[2],
                four = repositories[3],
                five = repositories[4],
            });
        }
    }
}