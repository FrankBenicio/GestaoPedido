using FluentAssertions;
using GestaoPedido.Data.UseCases.ProdutoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.ProdutoCases
{
    public class ListarProdutosUseCaseTests
    {
        private readonly Mock<IProdutoRepository> _repositoryMock;
        private readonly ListarProdutosUseCase _useCase;

        public ListarProdutosUseCaseTests()
        {
            _repositoryMock = new Mock<IProdutoRepository>();
            _useCase = new ListarProdutosUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_ShouldReturnPagedResultOfProdutoResponse()
        {
            // Arrange
            var produtos = new List<Produto>
            {
                Produto.Criar("Produto 1", 10m),
                Produto.Criar("Produto 2", 20m)
            };
            var pagedResult = new PagedResult<Produto>(produtos, 2, 1, 10);
            _repositoryMock
                .Setup(r => r.ObterTodosPaginadoAsync(1, 10))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _useCase.ExecutarAsync(1, 10);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalItems.Should().Be(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.Items.Select(x => x.Nome).Should().Contain(new[] { "Produto 1", "Produto 2" });
        }

        [Fact]
        public async Task ExecutarAsync_ShouldReturnEmptyPagedResult_WhenNoProdutos()
        {
            // Arrange
            var pagedResult = new PagedResult<Produto>(Enumerable.Empty<Produto>(), 0, 1, 10);
            _repositoryMock
                .Setup(r => r.ObterTodosPaginadoAsync(1, 10))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _useCase.ExecutarAsync(1, 10);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
    }
}
