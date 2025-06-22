using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Models.Enums;

namespace GestaoPedido.Data.UseCases.PedidoCases;

public sealed class ExcluirPedidoUseCase : IExcluirPedidoUseCase
{
    private readonly IUnitOfWork _uow;

    public ExcluirPedidoUseCase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task ExecutarAsync(Guid id)
    {
        var pedido = await _uow.Pedidos.ObterPorIdAsync(id)
                      ?? throw new KeyNotFoundException("Pedido não encontrado.");

        if (pedido.Status == StatusPedido.Pago)
            throw new ArgumentException("O pedido está pago e não pode ser excluído.");

        await _uow.ExecuteInTransactionAsync(async () =>
        {
            await _uow.Pedidos.RemoverAsync(pedido);
        });
    }
}

