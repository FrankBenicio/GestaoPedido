namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases
{
    public interface IExcluirPedidoUseCase
    {
        Task ExecutarAsync(Guid id);
    }

}
