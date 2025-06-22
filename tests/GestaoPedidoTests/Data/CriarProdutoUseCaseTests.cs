using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GestaoPedido.Data.UseCases.ProdutoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests;
using GestaoPedido.Domain.Models;
using Moq;
using Xunit;

namespace GestaoPedido.Data.Tests.UseCases.ProdutoCases
{
    public class CriarProdutoUseCaseTests
    {
        private readonly Mock<IValidator<CriarProdutoRequest>> _validatorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProdutoRepository> _produtoRepoMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly CriarProdutoUseCase _sut;

        public CriarProdutoUseCaseTests()
        {
            _validatorMock = new Mock<IValidator<CriarProdutoRequest>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _produtoRepoMock = new Mock<IProdutoRepository>();
            _userContextMock = new Mock<IUserContext>();

            _unitOfWorkMock.SetupGet(u => u.Produtos).Returns(_produtoRepoMock.Object);

            _sut = new CriarProdutoUseCase(
                _validatorMock.Object,
                _unitOfWorkMock.Object,
                _userContextMock.Object
            );
        }

        [Fact]
        public async Task ExecutarAsync_Should_AddProduto_When_ValidRequest()
        {
            // Arrange
            var request = new CriarProdutoRequest { Nome = "Produto Teste", Preco = 10.5m };
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());
            var userId = Guid.NewGuid();
            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);

            _unitOfWorkMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (func) => await func());

            // Act
            await _sut.ExecutarAsync(request);

            // Assert
            _produtoRepoMock.Verify(p => p.AdicionarAsync(It.Is<Produto>(prod =>
                prod.Nome == request.Nome &&
                prod.Preco == request.Preco
            )), Times.Once);
        }

        [Fact]
        public async Task ExecutarAsync_Should_ThrowValidationException_When_InvalidRequest()
        {
            // Arrange
            var request = new CriarProdutoRequest { Nome = "", Preco = -1 };
            var failures = new[] { new ValidationFailure("Nome", "Nome obrigatório") };
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            Func<Task> act = async () => await _sut.ExecutarAsync(request);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Nome obrigatório*");
            _produtoRepoMock.Verify(p => p.AdicionarAsync(It.IsAny<Produto>()), Times.Never);
        }
    }
}
