using FluentValidation;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests;
using GestaoPedido.Domain.Models;

namespace GestaoPedido.Data.UseCases.ClienteCases;

public class CriarClienteUseCase : ICriarClienteUseCase
{
    private readonly IValidator<CriarClienteRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CriarClienteUseCase(IValidator<CriarClienteRequest> validator, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task ExecutarAsync(CriarClienteRequest request)
    {
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
        var userId = _userContext.GetUserId();
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var cliente = Cliente.Criar(request.Nome, request.Documento);
            cliente.SetCreated(userId);
            await _unitOfWork.Clientes.Add(cliente);
        });
    }
}
