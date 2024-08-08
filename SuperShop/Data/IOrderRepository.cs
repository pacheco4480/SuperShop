using SuperShop.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IOrderRepository :IGenericRepository<Order>
    {
        //Na pratica isto será uma tarefa que devolve uma tabela de Orders para podermos ir buscar todas as encomendas
        //Este metodo dará todas as encomendas de um determinado User
        Task<IQueryable<Order>> GetOrderAsync(string userName);
    }
}
