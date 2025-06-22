using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoPedido.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly ICriarClienteUseCase _criarClienteUseCase;
    private readonly IObterClientePorIdUseCase _obterClientePorIdUseCase;
    private readonly IListarClientesUseCase _listarClientesUseCase;

    public ClientesController(
        ICriarClienteUseCase criarClienteUseCase,
        IObterClientePorIdUseCase obterClientePorIdUseCase,
        IListarClientesUseCase listarClientesUseCase)
    {
        _criarClienteUseCase = criarClienteUseCase;
        _obterClientePorIdUseCase = obterClientePorIdUseCase;
        _listarClientesUseCase = listarClientesUseCase;
    }

    /// <summary>
    /// Cria um novo cliente.
    /// </summary>
    /// <param name="request">Dados para criação do cliente.</param>
    /// <response code="200">Cliente criado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar cliente", Description = "Cria um novo cliente no sistema.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Cliente criado com sucesso.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação ou dados inválidos.")]
    public async Task<IActionResult> Post([FromBody] CriarClienteRequest request)
    {
        await _criarClienteUseCase.ExecutarAsync(request);
        return Ok("Cliente criado com sucesso.");
    }

    /// <summary>
    /// Obtém um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <response code="200">Cliente encontrado.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Buscar cliente por ID", Description = "Retorna os dados de um cliente específico pelo seu ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Cliente encontrado.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cliente não encontrado.")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cliente = await _obterClientePorIdUseCase.ExecutarAsync(id);
        return Ok(cliente);
    }

    /// <summary>
    /// Lista todos os clientes com paginação.
    /// </summary>
    /// <param name="pagina">Número da página.</param>
    /// <param name="tamanhoPagina">Tamanho da página.</param>
    /// <response code="200">Lista de clientes retornada com sucesso.</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar clientes", Description = "Lista os clientes existentes com suporte à paginação.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista de clientes retornada com sucesso.")]
    public async Task<IActionResult> GetAll([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
    {
        var clientes = await _listarClientesUseCase.ExecutarAsync(pagina, tamanhoPagina);
        return Ok(clientes);
    }
}
