using GestaoPedido.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedido.Infrastructure.Context
{
    public class GestaoPedidoContext(DbContextOptions<GestaoPedidoContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<PedidoProduto> PedidoProdutos => Set<PedidoProduto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PedidoProduto>()
                .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(pp => pp.PedidoId);

            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Produto)
                .WithMany()
                .HasForeignKey(pp => pp.ProdutoId);

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasIndex(p => p.Status)
                      .HasDatabaseName("IX_Pedido_Status");

                entity.HasIndex(p => p.Data)
                      .HasDatabaseName("IX_Pedido_Data");

                entity.HasIndex(p => p.ClienteId)
                      .HasDatabaseName("IX_Pedido_ClienteId");

                entity.HasIndex(p => new { p.Status, p.Data })
                      .HasDatabaseName("IX_Pedido_Status_Data");
            });


            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(c => c.Nome)
                      .HasDatabaseName("IX_Cliente_Nome");

                entity.HasIndex(c => c.Documento)
                      .IsUnique()
                      .HasDatabaseName("IX_Cliente_Documento");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasIndex(p => p.Nome)
                      .HasDatabaseName("IX_Produto_Nome");

                entity.HasIndex(p => p.Preco)
                      .HasDatabaseName("IX_Produto_Preco");
            });

        }
    }
}
