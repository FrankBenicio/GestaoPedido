using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases
{
    public interface IAtualizarPedidoUseCase
    {
        Task ExecutarAsync(Guid id, AtualizarPedidoRequest request);
    }

}
