using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Domain.Interfaces.Repositories
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido);


        Task<Pedido?> ObterPorIdAsync(Guid id);


        Task<PagedResult<Pedido>> ObterComFiltroPaginadoAsync(FiltroPedidoRequest filtro);


        Task AtualizarAsync(Pedido pedido);


        Task RemoverAsync(Pedido pedido);


        Task RemoverLogicamenteAsync(Pedido pedido);
    }

}
