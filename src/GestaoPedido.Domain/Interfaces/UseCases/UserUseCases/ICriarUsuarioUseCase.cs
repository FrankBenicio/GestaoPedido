using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;

namespace GestaoPedido.Domain.Interfaces.UseCases.UserUseCases;

public interface ICriarUsuarioUseCase
{
    Task Executar(CriarUsuarioRequest request);
}
