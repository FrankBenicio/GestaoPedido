using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using GestaoPedido.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedido.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GestaoPedidoContext _context;

    public UserRepository(GestaoPedidoContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }
}
