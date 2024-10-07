using Amazon.DynamoDBv2.Model;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using GloboClima.Domain.Models;
using GloboClima.Infra.CrossCutting.Security;
using GloboClima.Infra.CrossCutting.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Application.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IBaseService<User> _baseService;
        private readonly IConfiguration _configuration;

        public AuthController(
            IBaseService<User> baseService,
            IConfiguration configuration)
        {
            _baseService = baseService;
            _configuration = configuration;
        }

        /// <summary>
        /// Autentica um usuário.
        /// </summary>
        /// <param name="entity">Os dados do usuário contendo e-mail e senha.</param>
        /// <remarks>
        /// Este endpoint autentica o usuário utilizando o e-mail e a senha fornecidos.
        /// - Se o e-mail for inválido, retorna um **400 Bad Request** com a mensagem "E-mail Inválido".
        /// - Se o usuário não existir ou a senha for incorreta, retorna um **400 Bad Request** com a mensagem "Usuário inexistente".
        /// - Se as credenciais estiverem corretas, retorna um **200 OK** com o token JWT e as informações do usuário.
        /// </remarks>
        /// <returns>Um token JWT e as informações do usuário autenticado em caso de sucesso.</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Authenticate([FromBody] User entity)
        {
            IList<User> users = await _baseService.GetAsync();

            bool isValid = EmailValidator.IsValidEmail(entity.Email);
            if (isValid) {        
                
                User? userResponse = users.Where(x => x.Email == entity.Email).FirstOrDefault();
                if (userResponse != null)
                {
                    var passwordHash = SHA2.GenerateHash(entity.Password);
                    if (passwordHash == userResponse.Password)
                    {
                        TokenModel token = new Token(_configuration).GenerateJwtToken(userResponse);
                        
                        token.Name = userResponse.Name;
                        token.Email = userResponse.Email;

                        return Ok(token);
                    }
                }

                return BadRequest("Usuário inexistente");
            }

            return BadRequest("E-mail Inválido");

        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="entity">Os dados do usuário a serem registrados.</param>
        /// <remarks>
        /// Esta operação verifica se o e-mail é válido e se o usuário já existe no sistema antes de registrar um novo.
        /// - Se o e-mail for inválido, retorna um **400 Bad Request** com a mensagem "E-mail Inválido".
        /// - Se o usuário já existir, retorna um **400 Bad Request** com a mensagem "Usuário existente".
        /// - Se o registro for bem-sucedido, retorna um **200 OK** com os dados do usuário.
        /// </remarks>
        /// <returns>Retorna o status da operação e os dados do usuário registrado em caso de sucesso.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User entity)
        {
            bool isValid = EmailValidator.IsValidEmail(entity.Email);
            if (isValid) {             
                IList<User> users = await _baseService.GetAsync();
                bool userExists = users.Any(x => x.Email == entity.Email);

                if (!userExists)
                {
                    entity.Password = SHA2.GenerateHash(entity.Password);
                    await _baseService.AddAsync(entity);
                    return Ok(entity);
                }

                return BadRequest("Usuário existente");

            }

            return BadRequest("E-mail Inválido");

        }

    }
}
