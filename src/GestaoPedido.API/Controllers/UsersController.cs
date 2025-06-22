using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoPedido.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ICriarUsuarioUseCase _criarUsuarioUseCase;

    public UsersController(ICriarUsuarioUseCase criarUsuarioUseCase)
    {
        _criarUsuarioUseCase = criarUsuarioUseCase;
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="request">Dados para criação do usuário (username, senha, perfil).</param>
    /// <response code="200">Usuário criado com sucesso.</response>
    /// <response code="400">Dados inválidos ou usuário já existe.</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar usuário", Description = "Cria um novo usuário no sistema.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Usuário criado com sucesso.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação ou usuário já existente.")]
    public async Task<IActionResult> Post([FromBody] CriarUsuarioRequest request)
    {
        await _criarUsuarioUseCase.Executar(request);
        return Ok("Usuário criado com sucesso.");
    }
}
