using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Responses;

namespace GestaoPedido.Domain.Interfaces.UseCases.UserUseCases
{
    public interface IAutenticarUsuarioUseCase
    {
        Task<LoginResponse> ExecutarAsync(LoginRequest request);
    }
}
