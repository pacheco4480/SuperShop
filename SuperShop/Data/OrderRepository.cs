using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    //A nossa classe OrderRepository tem que herdar do GenericRepository mas tambem tem
    //que implementar o interface IOrderRepository temos entao que por este interface pois segundo
    //as regras do Dependecy Injection temos sempre que esepcificar qual é o interface primeiro e
    //depois é que é a classe, só pelo facto de termos que implementar o interface no ficheiro
    //Startup.cs nós obrigatoriamente temos que o por aqui, caso nao o pormos ele nao vai busca-lo
    //mesmo herdando do generico, mas nos nunca estamos a injetar o generico no Startup.cs.
    // Em resumo, ao implementar a interface aqui, asseguramos que o Startup.cs consiga identificar e mapear 
    // corretamente o serviço, permitindo que a aplicação injete a implementação correta onde for necessária.
    //Ctrl  + . em cima do IOrderRepository "implement interface"
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        // Construtor da classe OrderRepository que aceita um DataContext como parâmetro
        // e passa este contexto para a classe base (GenericRepository)
        //Ctrl  + . em cima do context e clicar em "Create and assign field context"
        //Ctrl  + . em cima do userHelper e clicar em "Create and assign field userHelper"
        public OrderRepository(DataContext context, IUserHelper userHelper) :base (context)
        {

            // O construtor chama o construtor da classe base com o parâmetro "context"
            // Isto inicializa o contexto na classe base GenericRepository
            _context = context;
            _userHelper = userHelper;
        }

        //Isto será apenas para ir buscar as encomendas
        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {   // Obtém o utilizador com base no email
            var user = await _userHelper.GetUserByEmailAsync(userName);
            // Se o utilizador não existir, retorna uma lista vazia
            if (user == null)
            {   
                return null;
            }
            //Se existir User e for Admin
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {   //Vai buscar todas as encomendas que lá estao
                //Vai à tabela Orders
                return _context.Orders
                    //Inclui todos os items da tabela Orders
                        .Include(o => o.Items)
                        //Temos que fazer ThenInclude porque a tabela Orders nao está
                        //diretamente ligada à tabela Products entre ela existe a tabela OrderDetails
                        //A tabela Orders será a tabela A, tabela OrdersDetails B e atabela Products C
                        //Entao sempre que queiramos ligar por exemplo a tabela A à tabela C usamos o ThenInclude
                        //Quando as tabelas estao diretamente ligadas basta usar só o .Include
                        .ThenInclude(p => p.Product)
                        .OrderByDescending(o => o.OrderDate);
            }
            // Se não for um administrador, retorna apenas as encomendas do utilizador específico
            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                // Filtra as encomendas pelo utilizador
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }
    }
}
