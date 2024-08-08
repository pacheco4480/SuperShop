using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SuperShop.Data.Entities;
using System.Data.Common;

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


        //Cntrl + . sobre "DbContextOptions" para injetarmos o DataContext da Entitie Framework Core
        //na nossa para ele conseguir reconhecer o "DbContextOptions"
        //Este DbContextOptions é o DataContext da Entitie Framework Core e para usarmos a nossa
        //temos que injetar <DataContext>
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
            
        
        }
    }
}
