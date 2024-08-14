using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        //1º Precisamos de ter o DataContext para o seed fazer as coisas este DataContext é
        //gerado atraves trl  + . que é feito no construtor em baixo
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        

        //Isto serve para gerar os produtos aleatoriamente
        private Random _random;

        //Ctrl  + . em cima do context e clicar em "Create and assign field context"
        //Ctrl  + . em cima do userHelper e clicar em "Create and assign field userHelper"
        //UserManager é a classe que manipula os utilizadores Se quisessemos por um User normal bastaria por UserManager,
        //mas neste caso queremos ir buscar
        //a nossa entidade onde adicionamos as propriedades FirstName e LastName temos entao que por UserManager<User>
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {   // Aplica todas as migrações pendentes à base de dados.
            // Se a base de dados não existir, cria-a antes de aplicar as migrações.
            await _context.Database.MigrateAsync();


            //Roles - Metodo para ver se o Role Admin existe, se nao existir cria
            await _userHelper.CheckRoleAsync("Admin");
            //Roles - Metodo para ver se o Role Customer existe, se nao existir cria
            await _userHelper.CheckRoleAsync("Customer");

            //Países e cidades
            // Verifica se existem países na base de dados
            if (!_context.Countries.Any())
            {
                // Cria uma lista de cidades para adicionar a um país
                var cities = new List<City>
                {
                    new City { Name = "Lisboa" },
                    new City { Name = "Porto" },
                    new City { Name = "Faro" }
                };

                // Adiciona um novo país com a lista de cidades
                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                // Salva as alterações na base de dados
                await _context.SaveChangesAsync();
            }

            //Primeiro ver se ja existe o utillizador ou nao (este utilizador vai ser o admin)
            var user = await _userHelper.GetUserByEmailAsync("david@gmail.com");

            //Se ele nao existir
            if (user == null)
            {//Vai criar o user
                user = new User
                {
                    FirstName = "David",
                    LastName = "Pacheco",
                    Email = "david@gmail.com",
                    UserName = "david@gmail.com",
                    PhoneNumber = "2155485",
                    Address = "Rua Jau 33",
                    //Países e cidades
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };

                //Utilizar a classe UserManager para criar o utilizador
                //"123456" - É a password, a pass está à parte nao estando no objeto User (no codigo em cima) para depois puder ser encriptada
                var result = await _userHelper.AddUserAsync(user, "123456");

                //Ver se foi criado ou nao
                if (result != IdentityResult.Success)
                { //Se nao conseguiu criar o User é apresentada a seguinte mensagem
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                //Roles - Adicionar o Role que ja existe ao User
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                //Email - Geramos o Token para poder confirmar
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //Email - Confirmamos o utilizador
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            //Roles - Este Role verifica se o User tem o Role que queremos chekar
            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            //Roles - Se o User que foi criado nao tem o Admin
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }



            //Depois de ter a base de dados feita cria os produtos para dentro da tabela
            //Se nao existirem produtos
            if (!_context.Products.Any())
            {
                //Método para cria produtos e associa os produtos ao utilizador
                AddProduct("Iphone X", user);
                AddProduct("Magic Mouse", user);
                AddProduct("iWatch Series 4", user);
                AddProduct("iPad Mini", user);
                //Gravar o produto na base de dados
                await _context.SaveChangesAsync();
            }
        }

        //Aqui vao ser criados os produtos
        private void AddProduct(string name, User user)
        {   //É aqui que vamos adicionar´à base de dados do produto
            //No (new product) ele inicialmente nao vai reconhecer o product Ctrl  + .  em cima de product e
            //selecionar Product - using SuperShop.Data.Entities
            _context.Products.Add(new Product
            {
                Name = name,
                //Cria um preço aleatorio até 1000
                Price = _random.Next(1000),
                //Por defeito está sempre disponivel
                IsAvailable = true,
                //stock aleatorio
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
