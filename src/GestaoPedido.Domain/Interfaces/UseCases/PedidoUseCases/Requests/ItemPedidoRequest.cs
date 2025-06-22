namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests
{
    public class ItemPedidoRequest
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
