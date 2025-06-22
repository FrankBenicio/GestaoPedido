using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases
{
    public interface IListarPedidosUseCase
    {
        Task<PagedResult<PedidoResponse>> ExecutarAsync(FiltroPedidoRequest filtro);
    }

}
