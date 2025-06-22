using FluentValidation;
using GestaoPedido.Data.Services;
using GestaoPedido.Data.UseCases.ClienteCases;
using GestaoPedido.Data.UseCases.PedidoCases;
using GestaoPedido.Data.UseCases.ProdutoCases;
using GestaoPedido.Data.UseCases.UserCases;
using GestaoPedido.Domain.Interfaces.Services;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;
using GestaoPedido.Domain.Options;
using GestaoPedido.Domain.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoPedido.Data;

public static class Cfg
{
    public static void ConfigureData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICriarUsuarioUseCase, CriarUsuarioUseCase>();
        services.AddScoped<IAutenticarUsuarioUseCase, AutenticarUsuarioUseCase>();
        services.AddScoped<IValidator<CriarUsuarioRequest>, CriarUsuarioRequestValidator>();

        services.AddScoped<ICriarClienteUseCase, CriarClienteUseCase>();
        services.AddScoped<IObterClientePorIdUseCase, ObterClientePorIdUseCase>();
        services.AddScoped<IListarClientesUseCase, ListarClientesUseCase>();
        services.AddScoped<IValidator<CriarClienteRequest>, CriarClienteRequestValidator>();

        services.AddScoped<ICriarProdutoUseCase, CriarProdutoUseCase>();
        services.AddScoped<IObterProdutoPorIdUseCase, ObterProdutoPorIdUseCase>();
        services.AddScoped<IListarProdutosUseCase, ListarProdutosUseCase>();
        services.AddScoped<IValidator<CriarProdutoRequest>, CriarProdutoRequestValidator>();

        services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
        services.AddScoped<IObterPedidoPorIdUseCase, ObterPedidoPorIdUseCase>();
        services.AddScoped<IListarPedidosUseCase, ListarPedidosUseCase>();
        services.AddScoped<IAtualizarPedidoUseCase, AtualizarPedidoUseCase>();
        services.AddScoped<IExcluirPedidoUseCase, ExcluirPedidoUseCase>();
        services.AddScoped<IExcluirPedidoLogicamenteUseCase, ExcluirPedidoLogicamenteUseCase>();

        services.AddScoped<IValidator<CriarPedidoRequest>, CriarPedidoRequestValidator>();
        services.AddScoped<IValidator<ItemPedidoRequest>, ItemPedidoRequestValidator>();
        services.AddScoped<IValidator<AtualizarPedidoRequest>, AtualizarPedidoRequestValidator>();


        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    }
}
