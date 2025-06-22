using GestaoPedido.Domain.Models;

namespace GestaoPedido.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsername(string username);
    Task Add(User user);
}
