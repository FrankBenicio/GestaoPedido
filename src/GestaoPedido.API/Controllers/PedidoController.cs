using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoPedido.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly ICriarPedidoUseCase _criarUseCase;
        private readonly IObterPedidoPorIdUseCase _obterPorIdUseCase;
        private readonly IListarPedidosUseCase _listarUseCase;
        private readonly IAtualizarPedidoUseCase _atualizarUseCase;
        private readonly IExcluirPedidoUseCase _excluirUseCase;
        private readonly IExcluirPedidoLogicamenteUseCase _excluirLogicoUseCase;

        public PedidoController(
            ICriarPedidoUseCase criarUseCase,
            IObterPedidoPorIdUseCase obterPorIdUseCase,
            IListarPedidosUseCase listarUseCase,
            IAtualizarPedidoUseCase atualizarUseCase,
            IExcluirPedidoUseCase excluirUseCase,
            IExcluirPedidoLogicamenteUseCase excluirLogicoUseCase)
        {
            _criarUseCase = criarUseCase;
            _obterPorIdUseCase = obterPorIdUseCase;
            _listarUseCase = listarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _excluirUseCase = excluirUseCase;
            _excluirLogicoUseCase = excluirLogicoUseCase;
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar pedido", Description = "Cria um novo pedido de compra.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Pedido criado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        public async Task<IActionResult> Post([FromBody] CriarPedidoRequest request)
        {
            var id = await _criarUseCase.ExecutarAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        /// <summary>
        /// Obtém os dados de um pedido pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar pedido por ID", Description = "Retorna os dados de um pedido específico.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Pedido encontrado.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pedido não encontrado.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pedido = await _obterPorIdUseCase.ExecutarAsync(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Lista todos os pedidos com filtros.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Listar pedidos", Description = "Lista os pedidos existentes com filtros por data e status.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de pedidos retornada com sucesso.")]
        public async Task<IActionResult> GetAll([FromQuery] FiltroPedidoRequest filtro)
        {
            var pedidos = await _listarUseCase.ExecutarAsync(filtro);
            return Ok(pedidos);
        }

        /// <summary>
        /// Atualiza os dados de um pedido existente.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar pedido", Description = "Atualiza as informações de um pedido existente.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Pedido atualizado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pedido não encontrado.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] AtualizarPedidoRequest request)
        {
            await _atualizarUseCase.ExecutarAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Exclui permanentemente um pedido.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir pedido fisicamente", Description = "Exclui definitivamente um pedido do banco de dados.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Pedido excluído com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pedido não encontrado.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _excluirUseCase.ExecutarAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Exclui logicamente um pedido.
        /// </summary>
        [HttpDelete("logical/{id}")]
        [SwaggerOperation(Summary = "Excluir pedido logicamente", Description = "Realiza uma exclusão lógica do pedido, mantendo os dados no banco.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Pedido inativado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pedido não encontrado.")]
        public async Task<IActionResult> DeleteLogical(Guid id)
        {
            await _excluirLogicoUseCase.ExecutarAsync(id);
            return NoContent();
        }
    }
}
