using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    // A interface IOrderRepository herda de IGenericRepository<Order>,
    // fornecendo operações CRUD genéricas para a entidade Order.
    // Além disso, ela define métodos específicos para a manipulação de pedidos.
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // Método assíncrono que retorna uma consulta de pedidos (Orders) para um determinado usuário.
        // Isso permite obter todas as encomendas associadas a um nome de usuário específico.
        Task<IQueryable<Order>> GetOrderAsync(string userName);

        // Método assíncrono que retorna uma consulta de itens de pedido temporários (OrderDetailTemp) para um determinado usuário.
        // Usado para gerenciar e visualizar o carrinho de compras temporário antes da finalização do pedido.
        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        // Método assíncrono que adiciona um item a um pedido, recebendo um modelo de item e um nome de usuário.
        // Implementa a lógica para adicionar produtos ao carrinho de compras de um usuário específico.
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        // Método assíncrono que modifica a quantidade de um item de pedido temporário.
        // Aceita o ID do item e a nova quantidade, permitindo atualizações de quantidade no carrinho de compras.
        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        // Método assíncrono que elimina um detalhe temporário de encomenda com base no ID fornecido.
        // Isto é útil para remover um item do carrinho de compras, por exemplo, quando um item é removido pelo utilizador.
        Task DeleteDetailTempAsync(int id);
    }
}
