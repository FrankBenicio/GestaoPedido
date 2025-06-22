using GestaoPedido.Domain.Interfaces.Repositories;
using GestaoPedido.Infrastructure.Context;
using GestaoPedido.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoPedido.Infrastructure
{
    public static class CfgInfrastructure
    {
        public static void ConfigureRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GestaoPedidoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                {
                    sqlOptions.CommandTimeout(60); 
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,          
                        maxRetryDelay: TimeSpan.FromSeconds(10), 
                        errorNumbersToAdd: null    
                    );
                })
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
        }
    }
}
