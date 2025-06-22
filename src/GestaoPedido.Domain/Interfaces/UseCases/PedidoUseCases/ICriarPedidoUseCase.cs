using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases
{
    public interface ICriarPedidoUseCase
    {
        Task<Guid> ExecutarAsync(CriarPedidoRequest request);
    }

}
