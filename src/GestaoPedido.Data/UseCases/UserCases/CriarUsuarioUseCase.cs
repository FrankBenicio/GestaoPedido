using FluentValidation;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using GestaoPedido.Domain.Models;

namespace GestaoPedido.Data.UseCases.UserCases;

public class CriarUsuarioUseCase : ICriarUsuarioUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CriarUsuarioRequest> _validator;

    public CriarUsuarioUseCase(IUnitOfWork unitOfWork, IValidator<CriarUsuarioRequest> validator, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Executar(CriarUsuarioRequest request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var usuarioExistente = await _unitOfWork.Users.GetByUsername(request.Username);
        if (usuarioExistente != null)
            throw new InvalidOperationException("Usuário já existe.");

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var user = User.Criar(request.Username, request.Password, request.Role);
            await _unitOfWork.Users.Add(user);
        });
    }
}

