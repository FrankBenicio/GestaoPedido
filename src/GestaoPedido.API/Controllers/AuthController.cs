using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoPedido.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAutenticarUsuarioUseCase _autenticarUsuarioUseCase;

        public AuthController(IAutenticarUsuarioUseCase autenticarUsuarioUseCase)
        {
            _autenticarUsuarioUseCase = autenticarUsuarioUseCase;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT.
        /// </summary>
        /// <param name="request">Credenciais do usuário (usuário e senha).</param>
        /// <returns>Token JWT e informações do usuário.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Autenticar usuário", Description = "Autentica um usuário com base nas credenciais fornecidas e retorna um token JWT.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Usuário autenticado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuário ou senha incorretos.")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _autenticarUsuarioUseCase.ExecutarAsync(request);
            return Ok(response);
        }
    }
}
