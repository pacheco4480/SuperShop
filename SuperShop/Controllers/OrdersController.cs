using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperShop.Data;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        //Construtor
        //Ctrl  + . em cima do orderRepository e clicar em "Create and assign field orderRepository"
        //Ctrl  + . em cima do productRepository e clicar em "Create and assign field productRepository"
        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        //Depois de elaborado este metodo temos que criar a respectiva View para o Index para isso
        //clicamos com o botao direito sobre Index() e fazemos Add View - Razor View - Add (Aqui podemos fazer uma 
        //View automatica depois do Add escolhemos estas opçoes Template - List, Model class - Order)
        public async Task<IActionResult> Index()
        {   
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);
            return View(model);
        }


        //Depois de elaborado este metodo temos que criar a respectiva View para o Create para isso
        //clicamos com o botao direito sobre Create() e fazemos Add View - Razor View - Add - (Template - Empty) - Add 
        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        //Depois de elaborado este metodo temos que criar a respectiva View para o AddProduct para isso
        //clicamos com o botao direito sobre AddProduct() e fazemos Add View - Razor View - Add - (Template - Empty) - Add 
        public IActionResult AddProduct()
        {
            // Cria uma nova instância do modelo AddItemViewModel
            var model = new AddItemViewModel
            {
                // Define a quantidade inicial do produto a ser adicionada como 1
                Quantity = 1,

                // Obtém uma lista de produtos como SelectListItem, que será utilizada para preencher um dropdown na view
                Products = _productRepository.GetComboProducts()
            };

            // Retorna a view associada ao método, passando o modelo criado como parâmetro
            return View(model);
        }

        [HttpPost]
        // Método para adicionar um produto a um pedido temporário
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            // Verifica se o modelo passado para o método é válido
            if (ModelState.IsValid)
            {
                // Chama o repositório de encomendas para adicionar o item ao pedido do utilizador atual
                await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);

                // Redireciona o utilizador para a ação "Create" após adicionar o item
                return RedirectToAction("Create");
            }

            // Se o modelo não for válido, retorna a mesma vista com o modelo fornecido para correção
            return View(model);
        }

        // Método assíncrono para eliminar um item de um pedido temporário com base no ID fornecido
        public async Task<IActionResult> DeleteItem(int? id)
        {
            // Verifica se o ID fornecido é nulo
            if (id == null)
            {
                // Se o ID for nulo, retorna um resultado "Não Encontrado" (404)
                return NotFound();
            }

            // Chama o repositório para eliminar o detalhe temporário de encomenda com o ID fornecido
            await _orderRepository.DeleteDetailTempAsync(id.Value);

            // Redireciona o utilizador para a ação "Create" após a remoção do item
            return RedirectToAction("Create");
        }

        // Método assíncrono para aumentar a quantidade de um item de um pedido temporário com base no ID fornecido
        public async Task<IActionResult> Increase(int? id)
        {
            // Verifica se o ID fornecido é nulo
            if (id == null)
            {
                // Se o ID for nulo, retorna um resultado "Não Encontrado" (404)
                return NotFound();
            }

            // Chama o repositório para aumentar a quantidade do detalhe temporário de encomenda com o ID fornecido
            // O aumento é de 1 unidade
            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);

            // Redireciona o utilizador para a ação "Create" após o aumento da quantidade
            return RedirectToAction("Create");
        }

        // Método assíncrono para diminuir a quantidade de um item de um pedido temporário com base no ID fornecido
        public async Task<IActionResult> Decrease(int? id)
        {
            // Verifica se o ID fornecido é nulo
            if (id == null)
            {
                // Se o ID for nulo, retorna um resultado "Não Encontrado" (404)
                return NotFound();
            }

            // Chama o repositório para diminuir a quantidade do detalhe temporário de encomenda com o ID fornecido
            // O decremento é de 1 unidade
            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);

            // Redireciona o utilizador para a ação "Create" após a diminuição da quantidade
            return RedirectToAction("Create");
        }

        // Método assíncrono para confirmar um pedido baseado no nome do utilizador
        public async Task<IActionResult> ConfirmOrder()
        {
            // Obtém o nome do utilizador atual a partir da identidade do utilizador
            // e chama o método ConfirmOrderAsync do repositório para confirmar o pedido
            var response = await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name);

            // Verifica se a confirmação do pedido foi bem-sucedida
            if (response)
            {
                // Se o pedido foi confirmado com sucesso, redireciona para a ação "Index"
                // A ação "Index" geralmente exibe uma lista de pedidos ou uma página de confirmação
                return RedirectToAction("Index");
            }

            // Se a confirmação do pedido falhar (por exemplo, se não houver itens no pedido),
            // redireciona o utilizador de volta para a ação "Create" para que possa ajustar o pedido
            return RedirectToAction("Create");
        }

    }
}
