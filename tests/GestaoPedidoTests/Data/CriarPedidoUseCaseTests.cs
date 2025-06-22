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
    public class CriarPedidoUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<CriarPedidoRequest>> _validatorMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IClienteRepository> _clienteRepoMock;
        private readonly Mock<IProdutoRepository> _produtoRepoMock;
        private readonly Mock<IPedidoRepository> _pedidoRepoMock;
        private readonly CriarPedidoUseCase _sut;

        public CriarPedidoUseCaseTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<CriarPedidoRequest>>();
            _userContextMock = new Mock<IUserContext>();
            _clienteRepoMock = new Mock<IClienteRepository>();
            _produtoRepoMock = new Mock<IProdutoRepository>();
            _pedidoRepoMock = new Mock<IPedidoRepository>();

            _uowMock.SetupGet(x => x.Clientes).Returns(_clienteRepoMock.Object);
            _uowMock.SetupGet(x => x.Produtos).Returns(_produtoRepoMock.Object);
            _uowMock.SetupGet(x => x.Pedidos).Returns(_pedidoRepoMock.Object);
            _uowMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

            _sut = new CriarPedidoUseCase(_uowMock.Object, _validatorMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_ShouldCreatePedido_WhenRequestIsValid()
        {
            // Arrange
            var clienteMock = Cliente.Criar("Cliente Teste", "123");
            var produtoMock = Produto.Criar("Produto Teste", 20);
            var clienteId = clienteMock.Id;
            var produtoId = produtoMock.Id;
            var userId = Guid.NewGuid();

            var request = new CriarPedidoRequest
            {
                ClienteId = clienteId,
                Itens = new List<ItemPedidoRequest>
            {
                new()
                {
                    ProdutoId = produtoId,
                    Quantidade = 3,
                    PrecoUnitario = 15.50m
                }
            }
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userContextMock.Setup(x => x.GetUserId()).Returns(userId);

            _uowMock.Setup(x => x.Clientes.ObterPorIdAsync(clienteId)).ReturnsAsync(clienteMock);
            _uowMock.Setup(x => x.Produtos.ObterPorIdAsync(produtoId)).ReturnsAsync(produtoMock);
            _uowMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                    .Returns<Func<Task>>(f => f());

            // Act
            var result = await _sut.ExecutarAsync(request);

            // Assert
            result.Should().NotBeEmpty("porque um novo pedido deve ser criado com um ID válido");
            _uowMock.Verify(x => x.Pedidos.AdicionarAsync(It.Is<Pedido>(p =>
                p.ClienteId == clienteId &&
                p.Itens.Count == 1 &&
                p.Itens.ToList()[0].ProdutoId == produtoId &&
                p.Itens.ToList()[0].Quantidade == 3 &&
                p.Itens.ToList()[0].PrecoUnitario == 15.50m
            )), Times.Once);
        }
    }
}
