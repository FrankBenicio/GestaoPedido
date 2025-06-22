namespace GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests
{
    public class CriarClienteRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
    }

}
