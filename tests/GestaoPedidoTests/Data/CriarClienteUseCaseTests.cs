using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GestaoPedido.Data.UseCases.ClienteCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests;
using GestaoPedido.Domain.Models;
using Moq;

namespace GestaoPedido.Data.Tests.UseCases.ClienteCases
{
    public class CriarClienteUseCaseTests
    {
        private readonly Mock<IValidator<CriarClienteRequest>> _validatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly CriarClienteUseCase _useCase;

        public CriarClienteUseCaseTests()
        {
            _validatorMock = new Mock<IValidator<CriarClienteRequest>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();

            _unitOfWorkMock.SetupGet(u => u.Clientes).Returns(_clienteRepositoryMock.Object);

            _useCase = new CriarClienteUseCase(
                _validatorMock.Object,
                _unitOfWorkMock.Object,
                _userContextMock.Object
            );
        }

        [Fact]
        public async Task ExecutarAsync_Should_ThrowValidationException_When_RequestIsInvalid()
        {
            // Arrange
            var request = new CriarClienteRequest { Nome = "", Documento = "" };
            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("Nome", "Nome is required") });
            _validatorMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(request);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task ExecutarAsync_Should_CreateCliente_When_RequestIsValid()
        {
            // Arrange
            var request = new CriarClienteRequest { Nome = "Cliente Teste", Documento = "123456789" };
            var validationResult = new FluentValidation.Results.ValidationResult();
            var userId = Guid.NewGuid();

            _validatorMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);
            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            _unitOfWorkMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (operation) =>
                {
                    await operation();
                });

            _clienteRepositoryMock.Setup(r => r.Add(It.IsAny<Cliente>())).Returns(Task.CompletedTask);

            // Act
            await _useCase.ExecutarAsync(request);

            // Assert
            _validatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once);
            _userContextMock.Verify(u => u.GetUserId(), Times.Once);
            _clienteRepositoryMock.Verify(r => r.Add(It.Is<Cliente>(c => c.Nome == request.Nome && c.Documento == request.Documento)), Times.Once);
            _unitOfWorkMock.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()), Times.Once);
        }
    }
}
