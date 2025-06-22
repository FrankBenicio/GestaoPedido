using System;
using System.Threading.Tasks;
using FluentAssertions;
using GestaoPedido.Data.UseCases.PedidoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Models;
using Moq;
using Xunit;

namespace GestaoPedido.Data.Tests.UseCases.PedidoCases
{
    public class ExcluirPedidoLogicamenteUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPedidoRepository> _pedidoRepoMock;
        private readonly ExcluirPedidoLogicamenteUseCase _useCase;

        public ExcluirPedidoLogicamenteUseCaseTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();
            _pedidoRepoMock = new Mock<IPedidoRepository>();

            _uowMock.SetupGet(u => u.Pedidos).Returns(_pedidoRepoMock.Object);

            _useCase = new ExcluirPedidoLogicamenteUseCase(_uowMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_Should_ExcluirPedidoLogicamente_And_SetUpdated()
        {
            // Arrange
            var clienteMock = Cliente.Criar("Cliente Teste", "123");
            var produtoMock = Produto.Criar("Produto Teste", 20);
            var clienteId = clienteMock.Id;
            var produtoId = produtoMock.Id;

            var pedidoId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var pedidoMock = Pedido.Criar(clienteId);
            _pedidoRepoMock.Setup(r => r.ObterPorIdAsync(pedidoId)).ReturnsAsync(pedidoMock);
            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);



            _uowMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (func) => await func());

            // Act
            await _useCase.ExecutarAsync(pedidoId);

            // Assert
            _pedidoRepoMock.Verify(r => r.AtualizarAsync(pedidoMock), Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_Should_Throw_When_Pedido_Not_Found()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            _pedidoRepoMock.Setup(r => r.ObterPorIdAsync(pedidoId)).ReturnsAsync((Pedido?)null);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(pedidoId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Pedido não encontrado.");
        }
    }
}
