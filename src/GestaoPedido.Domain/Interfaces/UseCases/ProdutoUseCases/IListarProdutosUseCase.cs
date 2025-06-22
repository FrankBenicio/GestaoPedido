using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Responses;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases
{
    public interface IListarProdutosUseCase
    {
        Task<PagedResult<ProdutoResponse>> ExecutarAsync(int pagina, int tamanhoPagina);
    }

}
