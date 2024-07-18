using SuperShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    //Esta Classe vai aceder ao DataContext
    public class Repository : IRepository
    {
        //1º Precisamos de ter o DataContext para o seed fazer as coisas este DataContext é
        //gerado atraves Ctrl  + . que é feito no construtor em baixo
        private readonly DataContext _context;


        //Se esta classe vai aceder ao Datacontext, temos que injetar aqui no construtor o Datacontext
        //Ctrl  + . em cima do context e clicar em "Create and assign field context"
        public Repository(DataContext context)
        {
            _context = context;
        }


        //Método que nos vai dar todos os produtos
        //Ctrl  + . em cima de <Product> para ir buscar a SuperShop.Data.Entities;
        public IEnumerable<Product> GetProducts()
        {   //retorna todos os produtos ordenados por nome
            return _context.Products.OrderBy(p => p.Name);
        }

        //Método que nos dá apenas um produto, recebe o id e devolve o produco com esse Id
        public Product GetProduct(int id)
        {
            return _context.Products.Find(id);
        }

        //Método que adiciona um Produto (aqui só adiciona o produto em memoria, nao adiciona na base de dados
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        //Método que atualiza um produto
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        //Método que remove um produto
        public void RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        //Método para gravar na base de dados
        public async Task<bool> SaveAllAsync()
        {   //Aqui grava tudo que está pendente para a base de dados
            return await _context.SaveChangesAsync() > 0;
        }

        //Método para ver se o produto existe
        public bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }
    }
}
