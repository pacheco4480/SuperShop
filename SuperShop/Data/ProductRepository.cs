using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data
{
    // A classe ProductRepository herda da classe genérica GenericRepository
    // e implementa a interface específica IProductRepository
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;

        // Construtor da classe ProductRepository que aceita um DataContext como parâmetro
        // e passa este contexto para a classe base (GenericRepository)
        public ProductRepository(DataContext context) : base(context)
        {
            // O construtor chama o construtor da classe base com o parâmetro "context"
            // Isto inicializa o contexto na classe base GenericRepository
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {   //Isto é como se tivessemos a fazer um Inner Join em SQl
            //Aqui relaciona as tabelas Produtos e Users
            return _context.Products.Include(p => p.User);
        }
    }
}
