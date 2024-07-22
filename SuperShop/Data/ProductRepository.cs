using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    // A classe ProductRepository herda da classe genérica GenericRepository
    // e implementa a interface específica IProductRepository
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        // Construtor da classe ProductRepository que aceita um DataContext como parâmetro
        // e passa este contexto para a classe base (GenericRepository)
        public ProductRepository(DataContext context) : base(context)
        {
            // O construtor chama o construtor da classe base com o parâmetro "context"
            // Isto inicializa o contexto na classe base GenericRepository
        }
    }
}
