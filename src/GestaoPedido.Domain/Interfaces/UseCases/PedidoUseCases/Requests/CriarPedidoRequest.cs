using GestaoPedido.Domain.Models.Enums;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;

public class CriarPedidoRequest
{
    public Guid ClienteId { get; set; }
    public List<ItemPedidoRequest> Itens { get; set; } = new();
}
