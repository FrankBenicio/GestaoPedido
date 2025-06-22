using GestaoPedido.Domain.Models.Enums;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests
{
    public class AtualizarPedidoRequest
    {
        public Guid ClienteId { get; set; }
        public DateTime Data { get; set; }
        public StatusPedido Status { get; set; }
        public List<ItemPedidoRequest> Itens { get; set; } = new();
    }

}
