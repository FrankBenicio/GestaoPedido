using FluentValidation;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Models;

namespace GestaoPedido.Data.UseCases.PedidoCases;

public sealed class CriarPedidoUseCase : ICriarPedidoUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CriarPedidoRequest> _validator;
    private readonly IUserContext _userContext;

    public CriarPedidoUseCase(IUnitOfWork uow, IValidator<CriarPedidoRequest> validator, IUserContext userContext)
    {
        _uow = uow;
        _validator = validator;
        _userContext = userContext;
    }

    public async Task<Guid> ExecutarAsync(CriarPedidoRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);

        var cliente = await _uow.Clientes.ObterPorIdAsync(request.ClienteId)
                      ?? throw new Exception("Cliente não encontrado.");
        var userId = _userContext.GetUserId();
        var pedido = Pedido.Criar(request.ClienteId);
        pedido.SetCreated(userId);
        foreach (var item in request.Itens)
        {
            var produto = await _uow.Produtos.ObterPorIdAsync(item.ProdutoId)
                          ?? throw new Exception($"Produto {item.ProdutoId} não encontrado.");

            pedido.AdicionarItem(produto.Id, item.Quantidade, item.PrecoUnitario);
        }

        await _uow.ExecuteInTransactionAsync(async () =>
        {
            await _uow.Pedidos.AdicionarAsync(pedido);
        });

        return pedido.Id;
    }
}
