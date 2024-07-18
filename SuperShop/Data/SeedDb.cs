using SuperShop.Data.Entities;
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

        //Isto serve para gerar os produtos aleatoriamente
        private Random _random;

        //Ctrl  + . em cima do context e clicar em "Create and assign field context"
        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {   //Vai ver se a base de dados está criada, se ela nao tiver criada, cria, se ja tiver criada, nao faz nada
            await _context.Database.EnsureCreatedAsync();

            //Depois de ter a base de dados feita cria os produtos para dentro da tabela
            //Se nao existirem produtos
            if (!_context.Products.Any())
            {
                //Método para cria produtos
                AddProduct("Iphone X");
                AddProduct("Magic Mouse");
                AddProduct("iWatch Series 4");
                AddProduct("iPad Mini");
                //Gravar o produto na base de dados
                await _context.SaveChangesAsync();
            }
        }

        //Aqui vao ser criados os produtos
        private void AddProduct(string name)
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
                Stock = _random.Next(100)
            });
        }
    }
}
