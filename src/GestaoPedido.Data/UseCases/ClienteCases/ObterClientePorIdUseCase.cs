using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases;
using GestaoPedido.Domain.Models;

namespace GestaoPedido.Data.UseCases.ClienteCases
{
    public class ObterClientePorIdUseCase : IObterClientePorIdUseCase
    {
        private readonly IClienteRepository _clienteRepository;

        public ObterClientePorIdUseCase(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> ExecutarAsync(Guid id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);

            if (cliente is null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            return cliente;
        }
    }

}
