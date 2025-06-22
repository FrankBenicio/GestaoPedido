using FluentAssertions;
using GestaoPedido.Data.UseCases.UserCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Options;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GestaoPedido.Data.Tests.UseCases.UserCases
{
    public class AutenticarUsuarioUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock = new();

        private readonly AutenticarUsuarioUseCase _useCase;

        public AutenticarUsuarioUseCaseTests()
        {
            var jwtSettings = new JwtSettings
            {
                Secret = "super_secret_test_key_1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiresInMinutes = 60
            };

            _jwtSettingsMock.Setup(x => x.Value).Returns(jwtSettings);

            _useCase = new AutenticarUsuarioUseCase(_uowMock.Object, _jwtSettingsMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarToken_QuandoCredenciaisForemValidas()
        {
            // Arrange
            var username = "admin";
            var senhaPlana = "123456";
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(senhaPlana);

            var user = User.Criar(username, senhaPlana, "Administrador");

            var request = new LoginRequest
            {
                Username = username,
                Password = senhaPlana
            };

            _uowMock.Setup(u => u.Users.GetByUsername(username)).ReturnsAsync(user);

            // Act
            var response = await _useCase.ExecutarAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.Token.Should().NotBeNullOrWhiteSpace();
            response.Username.Should().Be(username);
            response.Role.Should().Be("Administrador");
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_SeUsuarioNaoExistir()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "naoexiste",
                Password = "senha"
            };

            _uowMock.Setup(u => u.Users.GetByUsername(It.IsAny<string>())).ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Usuário ou senha inválidos.");
        }

        [Fact]
        public async Task ExecutarAsync_DeveLancarExcecao_SeSenhaForInvalida()
        {
            // Arrange
            var senhaCorreta = "correta";
            var senhaErrada = "errada";

            var user = User.Criar("username", senhaCorreta, "Administrador");

            var request = new LoginRequest
            {
                Username = user.Username,
                Password = senhaErrada
            };

            _uowMock.Setup(u => u.Users.GetByUsername(user.Username)).ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _useCase.ExecutarAsync(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Usuário ou senha inválidos.");
        }
    }
}
