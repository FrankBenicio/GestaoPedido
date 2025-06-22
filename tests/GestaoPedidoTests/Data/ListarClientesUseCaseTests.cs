using FluentAssertions;
using GestaoPedido.Data.UseCases.ClienteCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.ClienteCases
{
    public class ListarClientesUseCaseTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly ListarClientesUseCase _useCase;

        public ListarClientesUseCaseTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _useCase = new ListarClientesUseCase(_clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_ShouldReturnPagedResultOfClienteResponse()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                Cliente.Criar("Cliente 1", "123"),
                Cliente.Criar("Cliente 2", "456")
            };
            var pagedResult = new PagedResult<Cliente>(clientes, 2, 1, 10);

            _clienteRepositoryMock
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
            result.Items.Select(x => x.Nome).Should().Contain(new[] { "Cliente 1", "Cliente 2" });
        }

        [Fact]
        public async Task ExecutarAsync_ShouldReturnEmptyPagedResult_WhenNoClientes()
        {
            // Arrange
            var pagedResult = new PagedResult<Cliente>(Enumerable.Empty<Cliente>(), 0, 1, 10);

            _clienteRepositoryMock
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
