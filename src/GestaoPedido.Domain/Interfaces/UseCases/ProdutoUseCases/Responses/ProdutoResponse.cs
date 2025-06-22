using GestaoPedido.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Responses
{
    public class ProdutoResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        public ProdutoResponse() { }

        public ProdutoResponse(Produto produto)
        {
            Id = produto.Id;
            Nome = produto.Nome;
            Preco = produto.Preco;
        }
    }

}
