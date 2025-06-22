using FluentAssertions;
using GestaoPedido.Data.UseCases.ClienteCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Models;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.ClienteCases
{
    public class ObterClientePorIdUseCaseTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly ObterClientePorIdUseCase _useCase;

        public ObterClientePorIdUseCaseTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _useCase = new ObterClientePorIdUseCase(_clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarCliente_QuandoClienteExiste()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var cliente = Cliente.Criar("Cliente Teste", "12345678900");
            typeof(EntityBase).GetProperty(nameof(EntityBase.Id))!.SetValue(cliente, clienteId);
            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(clienteId))
                .ReturnsAsync(cliente);

            // Act
            var resultado = await _useCase.ExecutarAsync(clienteId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(clienteId);
            resultado.Nome.Should().Be("Cliente Teste");
            resultado.Documento.Should().Be("12345678900");
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_QuandoClienteNaoExiste()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(clienteId))
                .ReturnsAsync((Cliente?)null);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(clienteId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cliente não encontrado.");
        }
    }
}
