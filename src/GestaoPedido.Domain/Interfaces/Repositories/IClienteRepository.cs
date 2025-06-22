using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task Add(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task<PagedResult<Cliente>> ObterTodosPaginadoAsync(int pagina, int tamanhoPagina);
    }

}
