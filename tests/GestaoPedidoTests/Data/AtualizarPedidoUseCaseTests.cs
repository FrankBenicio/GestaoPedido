using FluentAssertions;
using FluentValidation;
using GestaoPedido.Data.UseCases.PedidoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Models;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.PedidoCases
{

    public class AtualizarPedidoUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IValidator<AtualizarPedidoRequest>> _validatorMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();
        private readonly AtualizarPedidoUseCase _useCase;

        public AtualizarPedidoUseCaseTests()
        {
            _useCase = new AtualizarPedidoUseCase(_uowMock.Object, _validatorMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveAtualizarPedidoComSucesso()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var clienteMock = Cliente.Criar("Cliente Teste", "123");
            var clienteId = clienteMock.Id;
            var produtoMock = Produto.Criar("Produto Teste", 10);
            var produtoId = produtoMock.Id;

            var request = new AtualizarPedidoRequest
            {
                Data = DateTime.UtcNow,
                Status = Domain.Models.Enums.StatusPedido.Pago,
                ClienteId = clienteId,
                Itens = new List<ItemPedidoRequest>
            {
                new() { ProdutoId = produtoId, Quantidade = 2, PrecoUnitario = 10 }
            }
            };

            var pedidoMock = Pedido.Criar(clienteMock.Id);

            var pedidoId = Guid.NewGuid();
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            _uowMock.Setup(u => u.Pedidos.ObterPorIdAsync(pedidoId)).ReturnsAsync(pedidoMock);
            _uowMock.Setup(u => u.Clientes.ObterPorIdAsync(clienteId)).ReturnsAsync(clienteMock);
            _uowMock.Setup(u => u.Produtos.ObterPorIdAsync(produtoId)).ReturnsAsync(produtoMock);

            _uowMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(f => f());

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(pedidoId, request);

            // Assert
            await act.Should().NotThrowAsync();
            pedidoMock.ClienteId.Should().Be(clienteId);
            pedidoMock.Itens.Should().HaveCount(1);
            pedidoMock.Itens.ToList()[0].ProdutoId.Should().Be(produtoId);
            pedidoMock.Itens.ToList()[0].Quantidade.Should().Be(2);
        }
    }

}
