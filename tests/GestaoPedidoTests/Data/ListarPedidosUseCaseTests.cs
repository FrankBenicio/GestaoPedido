using FluentAssertions;
using GestaoPedido.Data.UseCases.PedidoCases;
using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Models.Enums;
using GestaoPedido.Domain.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GestaoPedido.Data.Tests.UseCases.PedidoCases
{
    public class ListarPedidosUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly ListarPedidosUseCase _useCase;

        public ListarPedidosUseCaseTests()
        {
            _useCase = new ListarPedidosUseCase(_uowMock.Object);
        }

        [Fact]
        public async Task ExecutarAsync_DeveRetornarPedidosComPaginacao()
        {
            // Arrange
            var filtro = new FiltroPedidoRequest
            {
                PageNumber = 1,
                PageSize = 10,
                DataInicio = null,
                DataFim = null,
                Status = null
            };

            var pedido1 = Pedido.Criar(Guid.NewGuid());
            var pedido2 = Pedido.Criar(Guid.NewGuid());

            var pagedPedidos = new PagedResult<Pedido>(
                new List<Pedido> { pedido1, pedido2 },
                totalItems: 2,
                pageNumber: 1,
                pageSize: 10
            );

            _uowMock.Setup(x => x.Pedidos.ObterComFiltroPaginadoAsync(filtro))
                    .ReturnsAsync(pagedPedidos);

            // Act
            var result = await _useCase.ExecutarAsync(filtro);

            // Assert
            result.Should().NotBeNull();
            result.TotalItems.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
    }
}
