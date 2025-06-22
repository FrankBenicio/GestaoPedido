using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Domain.Interfaces.Repositories
{
    public interface IProdutoRepository
    {
        Task AdicionarAsync(Produto produto);
        Task<Produto?> ObterPorIdAsync(Guid id);
        Task<PagedResult<Produto>> ObterTodosPaginadoAsync(int pagina, int tamanhoPagina);
    }

}
