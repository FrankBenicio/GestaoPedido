using FluentValidation;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests;
using GestaoPedido.Domain.Models;

namespace GestaoPedido.Data.UseCases.ProdutoCases;

public class CriarProdutoUseCase : ICriarProdutoUseCase
{
    private readonly IValidator<CriarProdutoRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CriarProdutoUseCase(IValidator<CriarProdutoRequest> validator, IUnitOfWork uow, IUserContext userContext)
    {
        _validator = validator;
        _unitOfWork = uow;
        _userContext = userContext;
    }

    public async Task ExecutarAsync(CriarProdutoRequest request)
    {
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
        var userId = _userContext.GetUserId();
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var produto = Produto.Criar(request.Nome, request.Preco);
            produto.SetCreated(userId); 
            await _unitOfWork.Produtos.AdicionarAsync(produto);
        });
    }
}

