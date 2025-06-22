using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases
{
    public interface IObterPedidoPorIdUseCase
    {
        Task<PedidoResponse?> ExecutarAsync(Guid id);
    }

}
