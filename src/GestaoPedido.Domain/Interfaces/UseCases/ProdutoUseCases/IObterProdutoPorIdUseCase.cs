using GestaoPedido.Domain.Models;

namespace GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases
{
    public interface IObterProdutoPorIdUseCase
    {
        Task<Produto> ExecutarAsync(Guid id);
    }

}
