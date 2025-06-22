namespace GestaoPedido.Domain.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "Usuario";

    protected User() { }

    private User(Guid id, string username, string passwordHash, string role = "Usuario")
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }


    public static User Criar(string username, string senha, string role = "Usuario")
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(senha);
        return new User(Guid.NewGuid(), username, hash, role);
    }
}
