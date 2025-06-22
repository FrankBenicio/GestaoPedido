using FluentValidation;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;

namespace GestaoPedido.Data.UseCases.PedidoCases;

public sealed class AtualizarPedidoUseCase : IAtualizarPedidoUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<AtualizarPedidoRequest> _validator;
    private readonly IUserContext _userContext;

    public AtualizarPedidoUseCase(IUnitOfWork uow, IValidator<AtualizarPedidoRequest> validator, IUserContext userContext)
    {
        _uow = uow;
        _validator = validator;
        _userContext = userContext;
    }

    public async Task ExecutarAsync(Guid id, AtualizarPedidoRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);
        var userId = _userContext.GetUserId();

        var pedido = await _uow.Pedidos.ObterPorIdAsync(id)
                         ?? throw new Exception("Pedido não encontrado.");

        var cliente = await _uow.Clientes.ObterPorIdAsync(request.ClienteId)
                      ?? throw new Exception("Cliente não encontrado.");

        await _uow.ExecuteInTransactionAsync(async () =>
        {
            pedido.Atualizar(request.Data, request.Status, request.ClienteId);
            pedido.SetUpdated(userId);
            pedido.Itens.Clear();

            foreach (var item in request.Itens)
            {
                var produto = await _uow.Produtos.ObterPorIdAsync(item.ProdutoId)
                              ?? throw new Exception($"Produto {item.ProdutoId} não encontrado.");

                pedido.AdicionarItem(produto.Id, item.Quantidade, item.PrecoUnitario);
            }

            await _uow.Pedidos.AtualizarAsync(pedido);
        });
    }
}

