using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;
using GestaoPedido.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedido.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly GestaoPedidoContext _context;

        public ProdutoRepository(GestaoPedidoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<PagedResult<Produto>> ObterTodosPaginadoAsync(int pagina, int tamanhoPagina)
        {
            var query = _context.Produtos.AsNoTracking();

            var total = await query.CountAsync();
            var dados = await query
                .OrderBy(p => p.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return new PagedResult<Produto>(dados, total, pagina, tamanhoPagina);
        }
    }

}
