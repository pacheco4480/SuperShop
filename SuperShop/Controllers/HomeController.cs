using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //Esta action vai ser executada quando entrar por este caminho error/404
        [Route("error/404")]
        //Este action é só para mostrar a página do Error404
        //Depois de elaborado este metodo temos que criar a respectiva View para o Error404 para isso
        //clicamos com o botao direito sobre Error404() e fazemos Add View - Razor View - Add
        public IActionResult Error404()
        {
            return View();
        }
    }
}
