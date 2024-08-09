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

    }
}
