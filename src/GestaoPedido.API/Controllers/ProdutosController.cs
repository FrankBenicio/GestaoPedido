using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoPedido.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly ICriarProdutoUseCase _criarProdutoUseCase;
        private readonly IObterProdutoPorIdUseCase _obterProdutoPorIdUseCase;
        private readonly IListarProdutosUseCase _listarProdutosUseCase;

        public ProdutosController(
            ICriarProdutoUseCase criarProdutoUseCase,
            IObterProdutoPorIdUseCase obterProdutoPorIdUseCase,
            IListarProdutosUseCase listarProdutosUseCase)
        {
            _criarProdutoUseCase = criarProdutoUseCase;
            _obterProdutoPorIdUseCase = obterProdutoPorIdUseCase;
            _listarProdutosUseCase = listarProdutosUseCase;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="request">Objeto com os dados do produto.</param>
        /// <response code="200">Produto criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar produto", Description = "Cria um novo produto no sistema.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Produto criado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        public async Task<IActionResult> Post([FromBody] CriarProdutoRequest request)
        {
            await _criarProdutoUseCase.ExecutarAsync(request);
            return Ok("Produto criado com sucesso.");
        }

        /// <summary>
        /// Obtém os dados de um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <response code="200">Produto encontrado.</response>
        /// <response code="404">Produto não encontrado.</response>
        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Buscar produto por ID", Description = "Retorna os dados de um produto específico pelo seu ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Produto encontrado.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Produto não encontrado.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var produto = await _obterProdutoPorIdUseCase.ExecutarAsync(id);
            return Ok(produto);
        }

        /// <summary>
        /// Lista produtos com suporte à paginação.
        /// </summary>
        /// <param name="pagina">Número da página.</param>
        /// <param name="tamanhoPagina">Tamanho de itens por página.</param>
        /// <response code="200">Lista de produtos retornada com sucesso.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Listar produtos", Description = "Lista todos os produtos com paginação.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de produtos retornada com sucesso.")]
        public async Task<IActionResult> GetAll([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            var produtos = await _listarProdutosUseCase.ExecutarAsync(pagina, tamanhoPagina);
            return Ok(produtos);
        }
    }
}
