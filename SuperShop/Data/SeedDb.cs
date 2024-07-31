using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
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
        {   //Vai ver se a base de dados está criada, se ela nao tiver criada, cria, se ja tiver criada, nao faz nada
            await _context.Database.EnsureCreatedAsync();

            //Roles - Metodo para ver se o Role Admin existe, se nao existir cria
            await _userHelper.CheckRoleAsync("Admin");
            //Roles - Metodo para ver se o Role Customer existe, se nao existir cria
            await _userHelper.CheckRoleAsync("Customer");


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
                    PhoneNumber = "2155485"
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
