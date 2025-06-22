using FluentAssertions;
using GestaoPedido.Data.UseCases.PedidoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models.Enums;
using GestaoPedido.Domain.Models;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.PedidoCases
{

    public class ExcluirPedidoUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly ExcluirPedidoUseCase _useCase;

        public ExcluirPedidoUseCaseTests()
        {
            _useCase = new ExcluirPedidoUseCase(_uowMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveExcluirPedido_SeNaoEstiverPago()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = Pedido.Criar(Guid.NewGuid());

            _uowMock.Setup(x => x.Pedidos.ObterPorIdAsync(pedidoId)).ReturnsAsync(pedido);
            _uowMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                    .Returns<Func<Task>>(f => f());

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(pedidoId);

            // Assert
            await act.Should().NotThrowAsync();
            _uowMock.Verify(x => x.Pedidos.RemoverAsync(pedido), Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_SePedidoNaoForEncontrado()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            _uowMock.Setup(x => x.Pedidos.ObterPorIdAsync(pedidoId)).ReturnsAsync((Pedido?)null);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(pedidoId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage("Pedido não encontrado.");
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_SePedidoEstiverPago()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = Pedido.Criar(Guid.NewGuid());
            pedido.Atualizar(DateTime.UtcNow, StatusPedido.Pago, pedido.ClienteId);

            _uowMock.Setup(x => x.Pedidos.ObterPorIdAsync(pedidoId)).ReturnsAsync(pedido);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(pedidoId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("O pedido está pago e não pode ser excluído.");
        }
    }
}
