using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Data.UseCases.PedidoCases
{
    public sealed class ListarPedidosUseCase : IListarPedidosUseCase
    {
        private readonly IUnitOfWork _uow;

        public ListarPedidosUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<PedidoResponse>> ExecutarAsync(FiltroPedidoRequest filtro)
        {
            var result = await _uow.Pedidos.ObterComFiltroPaginadoAsync(filtro);

            var mapped = result.Items.Select(p => new PedidoResponse(p));

            return new PagedResult<PedidoResponse>(mapped, result.TotalItems, result.PageNumber, result.PageSize);
        }
    }
}
