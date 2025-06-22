using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GestaoPedido.Data.UseCases.UserCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using GestaoPedido.Domain.Models;
using Moq;
using Xunit;

namespace GestaoPedido.Data.Tests.UseCases.UserCases
{
    public class CriarUsuarioUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidator<CriarUsuarioRequest>> _validatorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CriarUsuarioUseCase _useCase;

        public CriarUsuarioUseCaseTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<CriarUsuarioRequest>>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _unitOfWorkMock.SetupGet(u => u.Users).Returns(_userRepositoryMock.Object);

            _useCase = new CriarUsuarioUseCase(_unitOfWorkMock.Object, _validatorMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Executar_Should_ThrowValidationException_When_RequestIsInvalid()
        {
            // Arrange
            var request = new CriarUsuarioRequest { Username = "", Password = "", Role = "" };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Username", "Required") });
            _validatorMock.Setup(v => v.Validate(request)).Returns(validationResult);

            // Act
            Func<Task> act = async () => await _useCase.Executar(request);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Executar_Should_ThrowInvalidOperationException_When_UserAlreadyExists()
        {
            // Arrange
            var username = "existing";
            var senhaPlana = "pass";
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(senhaPlana);

            var user = User.Criar(username, senhaPlana, "Administrador");

            var request = new CriarUsuarioRequest { Username = "existing", Password = "pass", Role = "Usuario" };
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            _userRepositoryMock.Setup(r => r.GetByUsername(request.Username)).ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _useCase.Executar(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Usuário já existe.");
        }

        [Fact]
        public async Task Executar_Should_CreateUser_When_RequestIsValid_And_UserDoesNotExist()
        {
            // Arrange
            var request = new CriarUsuarioRequest { Username = "newuser", Password = "pass", Role = "Usuario" };
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            _userRepositoryMock.Setup(r => r.GetByUsername(request.Username)).ReturnsAsync((User?)null);

            bool transactionCalled = false;
            _unitOfWorkMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (func) => { transactionCalled = true; await func(); });

            _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _useCase.Executar(request);

            // Assert
            transactionCalled.Should().BeTrue();
            _userRepositoryMock.Verify(r => r.Add(It.Is<User>(u =>
                u.Username == request.Username &&
                u.Role == request.Role)), Times.Once);
        }
    }
}
