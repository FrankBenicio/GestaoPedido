using GestaoPedido.Domain.Models;

namespace GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Responses
{
    public class ClienteResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;

        public ClienteResponse(Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Documento = cliente.Documento;
        }
    }

}
