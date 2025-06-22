using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace GestaoPedido.Infrastructure.Repositories;


public class UnitOfWork : IUnitOfWork
{
    private readonly GestaoPedidoContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPedidoRepository _pedidoRepository;

    public UnitOfWork(
        GestaoPedidoContext context,
        IUserRepository userRepository,
        IClienteRepository clienteRepository,
        IProdutoRepository produtoRepository,
        IPedidoRepository pedidoRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
        _pedidoRepository = pedidoRepository;
    }

    public IUserRepository Users => _userRepository;
    public IProdutoRepository Produtos => _produtoRepository;
    public IClienteRepository Clientes => _clienteRepository;
    public IPedidoRepository Pedidos => _pedidoRepository;

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await operation();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

