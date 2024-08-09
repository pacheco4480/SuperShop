using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
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

        // Recebe um modelo de item e o nome de utilizador para adicionar o item ao pedido temporário
        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            // Obtém o utilizador com base no email fornecido
            var user = await _userHelper.GetUserByEmailAsync(userName);

            // Verifica se o utilizador existe
            if (user == null)
            {
                // Se o utilizador não existir, sai do método sem fazer alterações
                return;
            }

            // Procura o produto na base de dados pelo ID fornecido no modelo
            var product = await _context.Products.FindAsync(model.ProductId);

            // Verifica se o produto existe
            if (product == null)
            {
                // Se o produto não existir, sai do método sem fazer alterações
                return;
            }

            // Verifica se já existe um registo de OrderDetailTemp para o utilizador e produto específicos
            var orderDetailTemp = await _context.OrderDetailsTemp
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();

            // Se não existir um registo de OrderDetailTemp, cria um novo
            if (orderDetailTemp == null)
            {
                // Cria um novo OrderDetailTemp com os dados do produto e do utilizador
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price, // Define o preço do produto
                    Product = product, // Associa o produto ao OrderDetailTemp
                    Quantity = model.Quantity, // Define a quantidade com base no modelo fornecido
                    User = user, // Associa o utilizador ao OrderDetailTemp
                };

                // Adiciona o novo OrderDetailTemp ao contexto da base de dados
                _context.OrderDetailsTemp.Add(orderDetailTemp);
            }
            // Se já existir um registo de OrderDetailTemp, atualiza a quantidade
            else
            {
                // Incrementa a quantidade do item existente com a quantidade do modelo
                orderDetailTemp.Quantity += model.Quantity;

                // Atualiza o registo existente no contexto da base de dados
                _context.OrderDetailsTemp.Update(orderDetailTemp);
            }

            // Guarda as alterações na base de dados de forma assíncrona
            await _context.SaveChangesAsync();
        }

        // Método assíncrono para confirmar uma encomenda para um utilizador específico
        // Retorna um valor booleano que indica se a confirmação da encomenda foi bem-sucedida ou não
        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            // Obtém o utilizador com base no email fornecido
            var user = await _userHelper.GetUserByEmailAsync(userName);

            // Se o utilizador não existir, retorna false indicando que a confirmação não foi bem-sucedida
            if (user == null)
            {
                return false;
            }

            // Obtém todos os detalhes temporários da encomenda associados ao utilizador
            var orderTmps = await _context.OrderDetailsTemp
                .Include(o => o.Product) // Inclui os produtos relacionados com os detalhes temporários
                .Where(o => o.User == user) // Filtra os detalhes temporários com base no utilizador
                .ToListAsync(); // Converte o resultado para uma lista

            // Se não houver detalhes temporários ou a lista for nula, retorna false
            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            // Converte os detalhes temporários para objetos OrderDetail
            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            // Cria uma nova encomenda com base nos detalhes
            var order = new Order
            {
                OrderDate = DateTime.UtcNow, // Define a data do pedido como a data atual
                User = user, // Define o utilizador associado à encomenda
                Items = details // Define os detalhes da encomenda
            };

            // Adiciona a nova encomenda ao contexto
            await CreateAsync(order);

            // Remove os detalhes temporários da base de dados
            _context.OrderDetailsTemp.RemoveRange(orderTmps);

            // Guarda as alterações no contexto de forma assíncrona
            await _context.SaveChangesAsync();

            // Retorna true indicando que a confirmação da encomenda foi bem-sucedida
            return true;
        }


        // Método assíncrono para eliminar um detalhe temporário de encomenda com base no ID fornecido
        public async Task DeleteDetailTempAsync(int id)
        {
            // Procura o detalhe da encomenda temporário pelo ID
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);

            // Verifica se o detalhe da encomenda temporário não existe
            if (orderDetailTemp == null)
            {
                // Se o detalhe não for encontrado, sai do método sem fazer alterações
                return;
            }

            // Remove o detalhe da encomenda temporário do contexto
            _context.OrderDetailsTemp.Remove(orderDetailTemp);

            // Guarda as alterações na base de dados de forma assíncrona
            await _context.SaveChangesAsync();
        }


        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            // Obtém o utilizador com base no email
            var user = await _userHelper.GetUserByEmailAsync(userName);
            // Se o utilizador não existir, retorna uma lista vazia
            if (user == null)
            {
                return null;
            }
            //Se o utilizador existir vai buscar os temporarios
            return _context.OrderDetailsTemp
                .Include(p => p.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
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
                        //Inclui todos os User's
                        .Include(o => o.User)
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

        // Método para modificar a quantidade de um item temporário no pedido
        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            // Procura o detalhe da encomenda temporário pelo ID
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);

            // Verifica se não existe nenhum detalhe de encomenda temporário com o ID fornecido
            if (orderDetailTemp == null)
            {
                // Sai do método se o detalhe de encomenda temporário não for encontrado
                return;
            }

            // Se o detalhe de encomenda temporário existe, altera a quantidade
            orderDetailTemp.Quantity += quantity;

            // Verifica se a nova quantidade é maior que zero
            if (orderDetailTemp.Quantity > 0)
            {
                // Atualiza o detalhe de encomenda temporário no contexto da base de dados
                _context.OrderDetailsTemp.Update(orderDetailTemp);

                // Guarda as alterações na base de dados de forma assíncrona
                await _context.SaveChangesAsync();
            }
            // Se a quantidade for menor ou igual a zero, não faz atualizações nem remove
        }

    }
}
