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
            //Serviço de Identidade do Utilizador
            //Estamos a usar a nossa entidade User mas a entidade IdentityRole é a do programa
            //Aqui estamos a usar a nosso entidade USer pois acresecntamos as propriedades FirstName e Lastname às propriedades predefenidadas
            //Caso nao quisessemos acrescentar propriedades às propriedades predefenidas bastava por "IdentityUser, IdentityRole"
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                // Configura o provedor de tokens para autenticação
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

                // Requer que o email do utilizador seja confirmado para poder fazer login
                cfg.SignIn.RequireConfirmedEmail = true;

                // Requer que cada usuário tenha um email único
                cfg.User.RequireUniqueEmail = true;

                // Configurações de senha - Definimos estas configurações para facilitar os testes,
                // mas para ambientes de produção, deve-se adotar configurações mais seguras.

                // Não requer que a senha contenha um dígito
                cfg.Password.RequireDigit = false;

                // Não requer um número mínimo de caracteres únicos na senha
                cfg.Password.RequiredUniqueChars = 0;

                // Não requer que a senha contenha caracteres maiúsculos
                cfg.Password.RequireUppercase = false;

                // Não requer que a senha contenha caracteres minúsculos
                cfg.Password.RequireLowercase = false;

                // Não requer que a senha contenha caracteres não alfanuméricos (como símbolos)
                cfg.Password.RequireNonAlphanumeric = false;

                // Define o comprimento mínimo da senha como 6 caracteres
                cfg.Password.RequiredLength = 6;
            })
             .AddDefaultTokenProviders() // Adiciona os provedores de token padrão usados para operações de autenticação
            .AddEntityFrameworkStores<DataContext>();    // Configura a utilização do Entity Framework para armazenar
                                                        // informações de identidade no banco de dados

            // Configura os serviços de autenticação da aplicação
            services.AddAuthentication()
                // Adiciona suporte para autenticação com Cookies
                .AddCookie()
                // Adiciona suporte para autenticação com JWT (JSON Web Tokens)
                .AddJwtBearer(cfg =>
                {
                    // Configura os parâmetros de validação do token JWT
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Define o emissor (issuer) do token JWT. O emissor deve ser o mesmo que foi usado para assinar o token.
                        ValidIssuer = this.Configuration["Tokens:Issuer"],

                        // Define a audiência (audience) do token JWT. A audiência é a parte que deve consumir o token.
                        ValidAudience = this.Configuration["Tokens:Audience"],

                        // Define a chave usada para assinar o token JWT. Esta chave deve ser a mesma que foi usada para gerar o token.
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });


            //Cria um serviço que utilize o nosso DataContext que vai usar o SQL Server com a connection string
            services.AddDbContext<DataContext>(cfg =>
            {   //Aqui vai buscar a connectionString que temos no ficheiro appsettings.json
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            //Aqui na pratica estamos a fazer o seguinte quando alguem perguntar pelo SeedDb ele vai cria-lo
            //Usamos o AddTransient pois so vai ser usado uma vez e depois de usado deixa de estar em memoria 
            //Este é criado e depois desaparece
            services.AddTransient<SeedDb>();

            services.AddScoped<IUserHelper, UserHelper>();
            //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
            //services.AddScoped<IImageHelper, ImageHelper>();
            //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
            services.AddScoped<IBlobHelper, BlobHelper>();

            services.AddScoped<IConverterHelper, ConverterHelper>();

            services.AddScoped<IMailHelper, MailHelper>();

            // Assim que detectar que é preciso um repositório de produtos, ele vai automaticamente criar uma instância de ProductRepository
            // Usamos AddScoped porque este serviço pode ser criado várias vezes durante o tempo de vida de uma requisição HTTP
            // Exemplo: se um usuário clicar na aba de produtos na navbar, ele cria um objeto ProductRepository
            // Se o usuário clicar novamente, ele apaga o objeto antigo e cria um novo objeto ProductRepository
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICountryRepository, CountryRepository>();

            //O NotAuthorized passa a usar a action que criamos no AccountController na vez de usar a action predefenida
            services.ConfigureApplicationCookie(options =>
            {   //Aqui é anulado o retorno que tinhamos criado no ficheiro AccountController
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

            //Quando nao encontrar a página vai à procura de um error
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //Será necessário para fazer o LOGIN
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
