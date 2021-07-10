using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CursoEFCore.Domain;
using System;
using System.Linq;

namespace CursoEFCore.Date
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data source=TIAGO-S145\\SQLEXPRESS;Initial Catalog=CursoEFCore;Integrated Security=true",
                p => p.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromMilliseconds(5),
                    errorNumbersToAdd: null
                    ));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder){
            foreach(var entity in modelBuilder.Model.GetEntityTypes()){
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType())
                    && !property.GetMaxLength().HasValue)
                    {
                        property.SetMaxLength(100);
                        //property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}