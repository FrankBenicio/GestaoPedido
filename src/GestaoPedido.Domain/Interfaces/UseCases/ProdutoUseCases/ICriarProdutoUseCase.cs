using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests;

namespace GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases
{
    public interface ICriarProdutoUseCase
    {
        Task ExecutarAsync(CriarProdutoRequest request);
    }

}
