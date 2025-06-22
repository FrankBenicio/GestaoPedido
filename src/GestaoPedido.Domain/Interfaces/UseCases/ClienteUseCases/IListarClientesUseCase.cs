using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Responses;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases
{
    public interface IListarClientesUseCase
    {
        Task<PagedResult<ClienteResponse>> ExecutarAsync(int pagina, int tamanhoPagina);
    }

}
