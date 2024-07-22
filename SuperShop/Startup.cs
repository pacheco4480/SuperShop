using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Cria um serviço que utilize o nosso DataContext que vai usar o SQL Server com a connection string
            services.AddDbContext<DataContext>(cfg =>
            {   //Aqui vai buscar a connectionString que temos no ficheiro appsettings.json
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            //Aqui na pratica estamos a fazer o seguinte quando alguem perguntar pelo SeedDb ele vai cria-lo
            //Usamos o AddTransient pois so vai ser usado uma vez e depois de usado deixa de estar em memoria 
            //Este é criado e depois desaparece
            services.AddTransient<SeedDb>();
            // Assim que detectar que é preciso um repositório de produtos, ele vai automaticamente criar uma instância de ProductRepository
            // Usamos AddScoped porque este serviço pode ser criado várias vezes durante o tempo de vida de uma requisição HTTP
            // Exemplo: se um usuário clicar na aba de produtos na navbar, ele cria um objeto ProductRepository
            // Se o usuário clicar novamente, ele apaga o objeto antigo e cria um novo objeto ProductRepository
            services.AddScoped<IProductRepository, ProductRepository>();


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
