using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data
{
    // A interface IProductRepository vai herdar da interface genérica IGenericRepository
    // e será específica para a entidade Product
    public interface IProductRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();

    }
}
