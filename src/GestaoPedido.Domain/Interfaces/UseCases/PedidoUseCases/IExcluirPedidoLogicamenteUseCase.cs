namespace GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases
{
    public interface IExcluirPedidoLogicamenteUseCase
    {
        Task ExecutarAsync(Guid id);
    }

}
