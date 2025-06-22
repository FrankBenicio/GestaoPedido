namespace GestaoPedido.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IClienteRepository Clientes { get; }
    IProdutoRepository Produtos { get; }
    IPedidoRepository Pedidos { get; }

    Task ExecuteInTransactionAsync(Func<Task> operation);

    Task SaveChangesAsync();
}

