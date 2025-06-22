using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases;
using GestaoPedido.Domain.Models;

namespace GestaoPedido.Data.UseCases.ProdutoCases
{
    public class ObterProdutoPorIdUseCase : IObterProdutoPorIdUseCase
    {
        private readonly IProdutoRepository _repository;

        public ObterProdutoPorIdUseCase(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Produto> ExecutarAsync(Guid id)
        {
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return produto;
        }
    }

}
