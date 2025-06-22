using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses;

namespace GestaoPedido.Data.UseCases.PedidoCases
{
    public sealed class ObterPedidoPorIdUseCase : IObterPedidoPorIdUseCase
    {
        private readonly IUnitOfWork _uow;

        public ObterPedidoPorIdUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PedidoResponse?> ExecutarAsync(Guid id)
        {
            var pedido = await _uow.Pedidos.ObterPorIdAsync(id)
                          ?? throw new KeyNotFoundException("Pedido não encontrado.");

            return new PedidoResponse(pedido);
        }
    }
}
