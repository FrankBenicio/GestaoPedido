using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests;

namespace GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases
{
    public interface ICriarClienteUseCase
    {
        Task ExecutarAsync(CriarClienteRequest request);
    }
}
