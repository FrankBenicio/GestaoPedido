using GestaoPedido.Domain.Models;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses
{
    public class PedidoResponse
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<ItemPedidoResponse> Itens { get; set; } = new();

        public PedidoResponse(Pedido pedido)
        {
            Id = pedido.Id;
            ClienteId = pedido.ClienteId;
            ClienteNome = pedido.Cliente?.Nome ?? string.Empty;
            Data = pedido.Data;
            Status = pedido.Status.ToString();
            Total = pedido.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

            Itens = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                ProdutoId = i.ProdutoId,
                ProdutoNome = i.Produto?.Nome ?? string.Empty,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Subtotal = i.Quantidade * i.PrecoUnitario
            }).ToList();
        }
    }

}
