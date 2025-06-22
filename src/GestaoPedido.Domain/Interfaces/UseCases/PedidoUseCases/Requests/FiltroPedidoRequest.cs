using GestaoPedido.Domain.Models.Enums;

namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests
{
    public class FiltroPedidoRequest
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public StatusPedido? Status { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
