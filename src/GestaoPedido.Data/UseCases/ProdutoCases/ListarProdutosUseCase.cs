using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Responses;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Data.UseCases.ProdutoCases
{
    public class ListarProdutosUseCase : IListarProdutosUseCase
    {
        private readonly IProdutoRepository _repository;

        public ListarProdutosUseCase(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ProdutoResponse>> ExecutarAsync(int pagina, int tamanhoPagina)
        {
            var resultado = await _repository.ObterTodosPaginadoAsync(pagina, tamanhoPagina);

            return new PagedResult<ProdutoResponse>(resultado.Items.Select(c => new ProdutoResponse(c)).ToList(), resultado.TotalItems, resultado.PageNumber, resultado.PageSize);
        }
    }

}
