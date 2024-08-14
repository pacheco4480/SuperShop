using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            {
                // Configura o provedor de tokens para autentica��o
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

                // Requer que o email do utilizador seja confirmado para poder fazer login
                cfg.SignIn.RequireConfirmedEmail = true;

                // Requer que cada usu�rio tenha um email �nico
                cfg.User.RequireUniqueEmail = true;

                // Configura��es de senha - Definimos estas configura��es para facilitar os testes,
                // mas para ambientes de produ��o, deve-se adotar configura��es mais seguras.

                // N�o requer que a senha contenha um d�gito
                cfg.Password.RequireDigit = false;

                // N�o requer um n�mero m�nimo de caracteres �nicos na senha
                cfg.Password.RequiredUniqueChars = 0;

                // N�o requer que a senha contenha caracteres mai�sculos
                cfg.Password.RequireUppercase = false;

                // N�o requer que a senha contenha caracteres min�sculos
                cfg.Password.RequireLowercase = false;

                // N�o requer que a senha contenha caracteres n�o alfanum�ricos (como s�mbolos)
                cfg.Password.RequireNonAlphanumeric = false;

                // Define o comprimento m�nimo da senha como 6 caracteres
                cfg.Password.RequiredLength = 6;
            })
             .AddDefaultTokenProviders() // Adiciona os provedores de token padr�o usados para opera��es de autentica��o
            .AddEntityFrameworkStores<DataContext>();    // Configura a utiliza��o do Entity Framework para armazenar
                                                        // informa��es de identidade no banco de dados

            // Configura os servi�os de autentica��o da aplica��o
            services.AddAuthentication()
                // Adiciona suporte para autentica��o com Cookies
                .AddCookie()
                // Adiciona suporte para autentica��o com JWT (JSON Web Tokens)
                .AddJwtBearer(cfg =>
                {
                    // Configura os par�metros de valida��o do token JWT
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Define o emissor (issuer) do token JWT. O emissor deve ser o mesmo que foi usado para assinar o token.
                        ValidIssuer = this.Configuration["Tokens:Issuer"],

                        // Define a audi�ncia (audience) do token JWT. A audi�ncia � a parte que deve consumir o token.
                        ValidAudience = this.Configuration["Tokens:Audience"],

                        // Define a chave usada para assinar o token JWT. Esta chave deve ser a mesma que foi usada para gerar o token.
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });


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

            services.AddScoped<IMailHelper, MailHelper>();

            // Assim que detectar que � preciso um reposit�rio de produtos, ele vai automaticamente criar uma inst�ncia de ProductRepository
            // Usamos AddScoped porque este servi�o pode ser criado v�rias vezes durante o tempo de vida de uma requisi��o HTTP
            // Exemplo: se um usu�rio clicar na aba de produtos na navbar, ele cria um objeto ProductRepository
            // Se o usu�rio clicar novamente, ele apaga o objeto antigo e cria um novo objeto ProductRepository
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICountryRepository, CountryRepository>();

            //O NotAuthorized passa a usar a action que criamos no AccountController na vez de usar a action predefenida
            services.ConfigureApplicationCookie(options =>
            {   //Aqui � anulado o retorno que tinhamos criado no ficheiro AccountController
                //em public async Task<IActionResult> Login (LoginViewModel model) 
                options.LoginPath = "/Account/NotAuthorized";
                //Quando houver um acesso negado executa este controlador com esta action
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });


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
                app.UseExceptionHandler("/Errors/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Quando nao encontrar a p�gina vai � procura de um error
            app.UseStatusCodePagesWithReExecute("/error/{0}");

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
