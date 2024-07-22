using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    // A interface IProductRepository vai herdar da interface genérica IGenericRepository
    // e será específica para a entidade Product
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Neste ponto, não há necessidade de adicionar métodos adicionais porque
        // IProductRepository já herda todos os métodos de IGenericRepository para o tipo Product
    }
}
