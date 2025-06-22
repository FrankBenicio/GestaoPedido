namespace GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;

public class CriarUsuarioRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Usuario";
}
