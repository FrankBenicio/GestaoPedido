namespace GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests
{
    public class CriarProdutoRequest
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}
