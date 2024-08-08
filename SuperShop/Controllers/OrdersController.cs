using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        //Construtor
        //Ctrl  + . em cima do orderRepository e clicar em "Create and assign field orderRepository"
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        //Depois de elaborado este metodo temos que criar a respectiva View para o Index para isso
        //clicamos com o botao direito sobre Index() e fazemos Add View - Razor View - Add (Aqui podemos fazer uma 
        //View automatica depois do Add escolhemos estas opçoes Template - List, Model class - Order)
        public async Task<IActionResult> Index()
        {   
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);
            return View(model);
        }
    }
}
