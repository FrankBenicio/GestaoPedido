namespace GestaoPedido.Domain.Models
{
    public class PedidoProduto
    {
        public Guid PedidoId { get; private set; }
        public Pedido Pedido { get; private set; } = null!;

        public Guid ProdutoId { get; private set; }
        public Produto Produto { get; private set; } = null!;

        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        protected PedidoProduto() { }

        public PedidoProduto(Guid pedidoId, Guid produtoId, int quantidade, decimal precoUnitario)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }
    }

}
