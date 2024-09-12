using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SuperShop.Data.Entities;
using System.Data.Common;
using System.Linq;

namespace SuperShop.Data
{
    //Para pudermos usar a herança temos que dizer que a nossa DataContext vai herdar da deles IdentityDbContext e injeta o nosso User
    //"Ctrl + ." em IdentityDbContext e instalar Microsoft.AspNetCore.Identity.EntityFrameworkCore
    public class DataContext : IdentityDbContext<User>
    {

        //Propriedade que vai ficar responsável pela tabela
        //DbSet cria a tabela
        //Resumindo isto é a propriedade que vai ficar ligada à tabela Products quando ela for
        //criada através do DataContext
        public DbSet<Product> Products { get; set; }

        // Propriedade que representa a tabela de pedidos no banco de dados.
        // DbSet é responsável por criar e gerenciar a tabela Orders.
        public DbSet<Order> Orders { get; set; }

        // Propriedade que representa a tabela de detalhes do pedido no banco de dados.
        // DbSet é responsável por criar e gerenciar a tabela OrderDetails.
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // Propriedade que representa uma tabela temporária de detalhes do pedido no banco de dados.
        // DbSet é responsável por criar e gerenciar a tabela OrderDetailsTemp.
        public DbSet<OrderDetailTemp> OrderDetailsTemp { get; set; }

        // Propriedade que representa a tabela Countries no banco de dados.
        // DbSet<Country> é responsável por criar e gerenciar a tabela de países.
        public DbSet<Country> Countries { get; set; }

        // Propriedade que representa a tabela Cities no banco de dados.
        // DbSet<City> é responsável por criar e gerenciar a tabela de cidades.
        public DbSet<City> Cities { get; set; }


        //Cntrl + . sobre "DbContextOptions" para injetarmos o DataContext da Entitie Framework Core
        //na nossa para ele conseguir reconhecer o "DbContextOptions"
        //Este DbContextOptions é o DataContext da Entitie Framework Core e para usarmos a nossa
        //temos que injetar <DataContext>
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
            
        
        }

        // Método OnModelCreating é utilizado para personalizar o mapeamento das entidades para o banco de dados.
        // Aqui, podemos configurar restrições, índices e tipos de dados específicos para colunas
        // substituindo o tipo de dados que sao criados por padaro.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura um índice único na coluna Name da tabela Countries.
            // Isto garante que não existam países com nomes duplicados.
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Configura o tipo de dados da coluna Price na tabela Products.
            // O tipo decimal(18,2) indica que o preço terá até 18 dígitos no total, com 2 casas decimais.
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Configura o tipo de dados da coluna Price na tabela OrderDetailsTemp.
            // O tipo decimal(18,2) é usado para garantir a precisão dos valores de preços.
            modelBuilder.Entity<OrderDetailTemp>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18,2)");

            // Configura o tipo de dados da coluna Price na tabela OrderDetails.
            // O tipo decimal(18,2) é consistente para todas as tabelas que lidam com preços.
            modelBuilder.Entity<OrderDetail>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            // Chama o método base OnModelCreating da classe IdentityDbContext para garantir que
            // as configurações padrão do Identity também sejam aplicadas.
            base.OnModelCreating(modelBuilder);
        }


        // Habilitar a regra de apagar em cascata (Cascade Delete Rule)
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Obtém todas as chaves estrangeiras no modelo que têm comportamento de exclusão em cascata.
        //    var cascadeFKs = modelBuilder.Model
        //        .GetEntityTypes() // Procura todas as tabelas.
        //        .SelectMany(t => t.GetForeignKeys()) // Seleciona todas as chaves estrangeiras.
        //        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade); // Filtra as que têm comportamento em cascata.

        //    // Altera o comportamento de exclusão de cascata para restrito para todas as chaves estrangeiras filtradas.
        //    foreach (var fk in cascadeFKs)
        //    {
        //        fk.DeleteBehavior = DeleteBehavior.Restrict;
        //    }

        //    // Chama o método base para garantir que outras configurações do modelo sejam aplicadas.
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
