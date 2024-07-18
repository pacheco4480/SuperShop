using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Primeiro constroi o Host
            var host = CreateHostBuilder(args).Build();
            //Agora no host vai correr o Seeding, sendo que o Seeding significa
            //se nao existir uma base de dados ele cria, criando tambem as tabelas populando-as
            //e caso já exista uma base de dados ele nao cria a base de dados.
            RunSeeding(host);
            //Chegando aqui corre o host com tudo montado
            host.Run();
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
