using FluentAssertions;
using GestaoPedido.Data.UseCases.PedidoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.PedidoCases
{
    public class ObterPedidoPorIdUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly ObterPedidoPorIdUseCase _useCase;

        public ObterPedidoPorIdUseCaseTests()
        {
            _useCase = new ObterPedidoPorIdUseCase(_uowMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarPedidoResponse_SeEncontrado()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = Pedido.Criar(Guid.NewGuid());

            _uowMock.Setup(x => x.Pedidos.ObterPorIdAsync(pedidoId))
                    .ReturnsAsync(pedido);

            // Act
            var result = await _useCase.ExecutarAsync(pedidoId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(pedido.Id);
            result.ClienteId.Should().Be(pedido.ClienteId);
            result.Status.Should().Be(pedido.Status.ToString());
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_SePedidoNaoForEncontrado()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();

            _uowMock.Setup(x => x.Pedidos.ObterPorIdAsync(pedidoId))
                    .ReturnsAsync((Pedido?)null);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(pedidoId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage("Pedido não encontrado.");
        }
    }
}
