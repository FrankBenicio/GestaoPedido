using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;

namespace GestaoPedido.Data.UseCases.PedidoCases;

public sealed class ExcluirPedidoLogicamenteUseCase : IExcluirPedidoLogicamenteUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly IUserContext _userContext;

    public ExcluirPedidoLogicamenteUseCase(IUnitOfWork uow, IUserContext userContext)
    {
        _uow = uow;
        _userContext = userContext;
    }

    public async Task ExecutarAsync(Guid id)
    {
        var pedido = await _uow.Pedidos.ObterPorIdAsync(id)
                      ?? throw new KeyNotFoundException("Pedido não encontrado.");
        var userId = _userContext.GetUserId();
        pedido.Excluir();
        pedido.SetUpdated(userId);

        await _uow.ExecuteInTransactionAsync(async () =>
        {
            await _uow.Pedidos.AtualizarAsync(pedido);
        });
    }
}

