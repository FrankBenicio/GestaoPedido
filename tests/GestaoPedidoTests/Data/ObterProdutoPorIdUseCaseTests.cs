using System;
using System.Threading.Tasks;
using FluentAssertions;
using GestaoPedido.Data.UseCases.ProdutoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using Moq;
using Xunit;

namespace GestaoPedido.Data.Tests.UseCases.ProdutoCases
{
    public class ObterProdutoPorIdUseCaseTests
    {
        private readonly Mock<IProdutoRepository> _repositoryMock;
        private readonly ObterProdutoPorIdUseCase _useCase;

        public ObterProdutoPorIdUseCaseTests()
        {
            _repositoryMock = new Mock<IProdutoRepository>();
            _useCase = new ObterProdutoPorIdUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarProduto_QuandoProdutoExiste()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var produto = Produto.Criar("Produto Teste", 10.5m);
            typeof(EntityBase).GetProperty(nameof(EntityBase.Id))!.SetValue(produto, produtoId);
            _repositoryMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync(produto);

            // Act
            var resultado = await _useCase.ExecutarAsync(produtoId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Nome.Should().Be("Produto Teste");
            resultado.Preco.Should().Be(10.5m);
            resultado.Id.Should().Be(produtoId);
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_QuandoProdutoNaoExiste()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync((Produto?)null);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(produtoId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Produto não encontrado.");
        }
    }
}
