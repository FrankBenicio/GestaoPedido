using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Responses;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Responses;
using GestaoPedido.Domain.Models;
using GestaoPedido.Domain.Utils;

namespace GestaoPedido.Data.UseCases.ClienteCases;

public class ListarClientesUseCase : IListarClientesUseCase
{
    private readonly IClienteRepository _clienteRepository;

    public ListarClientesUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<PagedResult<ClienteResponse>> ExecutarAsync(int pagina, int tamanhoPagina)
    {
        var resultado = await _clienteRepository.ObterTodosPaginadoAsync(pagina, tamanhoPagina);

        return new PagedResult<ClienteResponse>(resultado.Items.Select(c => new ClienteResponse(c)).ToList(), resultado.TotalItems, resultado.PageNumber, resultado.PageSize);
    }

}
