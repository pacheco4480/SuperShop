using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
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
            //Servi�o de Identidade do Utilizador
            //Estamos a usar a nossa entidade User mas a entidade IdentityRole � a do programa
            //Aqui estamos a usar a nosso entidade USer pois acresecntamos as propriedades FirstName e Lastname �s propriedades predefenidadas
            //Caso nao quisessemos acrescentar propriedades �s propriedades predefenidas bastava por "IdentityUser, IdentityRole"
            services.AddIdentity<User, IdentityRole>(cfg =>
            {   //Aqui teoricamente deveria ser tudo true para fortelecer a password mas vamos optar por fazer
                //assim para puder testar mais facilmente a cria�ao dos utilizadores
                //Nao pode haver email repetidos
                cfg.User.RequireUniqueEmail = true;

                cfg.Password.RequireDigit = false;

                cfg.Password.RequiredUniqueChars = 0;

                cfg.Password.RequireUppercase = false;

                cfg.Password.RequireLowercase = false;

                cfg.Password.RequireNonAlphanumeric = false;

                cfg.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<DataContext>();


            //Cria um servi�o que utilize o nosso DataContext que vai usar o SQL Server com a connection string
            services.AddDbContext<DataContext>(cfg =>
            {   //Aqui vai buscar a connectionString que temos no ficheiro appsettings.json
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            //Aqui na pratica estamos a fazer o seguinte quando alguem perguntar pelo SeedDb ele vai cria-lo
            //Usamos o AddTransient pois so vai ser usado uma vez e depois de usado deixa de estar em memoria 
            //Este � criado e depois desaparece
            services.AddTransient<SeedDb>();

            services.AddScoped<IUserHelper, UserHelper>();
            //SUBSTITUI�AO IMAGEURL por IMAGEID - OLD
            //services.AddScoped<IImageHelper, ImageHelper>();
            //SUBSTITUI�AO IMAGEURL por IMAGEID - NEW
            services.AddScoped<IBlobHelper, BlobHelper>();

            services.AddScoped<IConverterHelper, ConverterHelper>();

            // Assim que detectar que � preciso um reposit�rio de produtos, ele vai automaticamente criar uma inst�ncia de ProductRepository
            // Usamos AddScoped porque este servi�o pode ser criado v�rias vezes durante o tempo de vida de uma requisi��o HTTP
            // Exemplo: se um usu�rio clicar na aba de produtos na navbar, ele cria um objeto ProductRepository
            // Se o usu�rio clicar novamente, ele apaga o objeto antigo e cria um novo objeto ProductRepository
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
            //Ser� necess�rio para fazer o LOGIN
            app.UseAuthentication();

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
