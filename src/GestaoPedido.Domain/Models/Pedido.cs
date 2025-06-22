using GestaoPedido.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestaoPedido.Domain.Models
{
    public class Pedido : EntityBase
    {
        public DateTime Data { get; private set; }
        public StatusPedido Status { get; private set; } = StatusPedido.Pendente;
        public bool Ativo { get; private set; } = true;

        public Guid ClienteId { get; private set; }
        public Cliente Cliente { get; private set; } = null!;

        public ICollection<PedidoProduto> Itens { get; private set; } = new List<PedidoProduto>();

        protected Pedido() { }

        private Pedido(Guid clienteId, DateTime data, StatusPedido status)
        {
            ClienteId = clienteId;
            Data = data;
            Status = status;
            Ativo = true;
        }

        public static Pedido Criar(Guid clienteId)
        {
            return new Pedido(clienteId, DateTime.Now, StatusPedido.Pendente);
        }

        public void AdicionarItem(Guid produtoId, int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
            if (precoUnitario <= 0) throw new ArgumentException("Preço unitário deve ser maior que zero.");

            Itens.Add(new PedidoProduto(Id, produtoId, quantidade, precoUnitario));
        }

        public void AtualizarStatus(StatusPedido status) => Status = status;

        public void Atualizar(DateTime data, StatusPedido status, Guid clienteId)
        {
            if (data > DateTime.UtcNow)
                throw new ArgumentException("A data do pedido não pode ser futura.");

            ClienteId = clienteId;
            Data = data;
            Status = status;
        }

        public void Excluir()
        {
            if (Status == StatusPedido.Pago)
                throw new ArgumentException("O pedido está pago e não pode excluído.");

            Ativo = false;
        }
    }


}
