namespace GestaoPedido.Domain.Models
{
    public class Produto : EntityBase
    {
        public string Nome { get; private set; } = string.Empty;
        public decimal Preco { get; private set; }
        public bool Ativo { get; private set; } = true;

        protected Produto() { }

        private Produto(string nome, decimal preco)
        {
            Nome = nome;
            Preco = preco;
            Ativo = true;
        }

        public static Produto Criar(string nome, decimal preco)
        {
            return new Produto(nome, preco);
        }

    }

}
