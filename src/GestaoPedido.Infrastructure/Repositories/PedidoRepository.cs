using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;
using GestaoPedido.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedido.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly GestaoPedidoContext _context;

        public PedidoRepository(GestaoPedidoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);;
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Itens)
                    .ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResult<Pedido>> ObterComFiltroPaginadoAsync(FiltroPedidoRequest filtro)
        {
            var query = _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Itens)
                    .ThenInclude(pp => pp.Produto)
                .AsQueryable();

            if (filtro.DataInicio.HasValue)
                query = query.Where(p => p.Data >= filtro.DataInicio.Value);

            if (filtro.DataFim.HasValue)
                query = query.Where(p => p.Data <= filtro.DataFim.Value);

            if (filtro.Status.HasValue)
                query = query.Where(p => p.Status == filtro.Status.Value);

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.Data)
                .Skip((filtro.PageNumber - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToListAsync();

            return new PagedResult<Pedido>(items, totalItems, filtro.PageNumber, filtro.PageSize);
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);;
        }

        public async Task RemoverAsync(Pedido pedido)
        {
            _context.Pedidos.Remove(pedido);;
        }

        public async Task RemoverLogicamenteAsync(Pedido pedido)
        {
            pedido.Excluir();

            _context.Pedidos.Update(pedido);;
        }
    }

}
