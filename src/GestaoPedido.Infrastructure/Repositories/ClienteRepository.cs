using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;
using GestaoPedido.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedido.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly GestaoPedidoContext _context;

        public ClienteRepository(GestaoPedidoContext context)
        {
            _context = context;
        }

        public async Task Add(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<PagedResult<Cliente>> ObterTodosPaginadoAsync(int pagina, int tamanhoPagina)
        {
            var query = _context.Clientes.AsNoTracking();

            var totalRegistros = await query.CountAsync();

            var clientes = await query
                .OrderBy(c => c.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return new PagedResult<Cliente>(clientes, totalRegistros, pagina, tamanhoPagina);
        }

    }

}
