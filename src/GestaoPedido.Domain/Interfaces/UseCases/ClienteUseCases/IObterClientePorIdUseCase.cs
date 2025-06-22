using GestaoPedido.Domain.Models;

namespace GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases
{
    public interface IObterClientePorIdUseCase
    {
        Task<Cliente?> ExecutarAsync(Guid id);
    }

}
