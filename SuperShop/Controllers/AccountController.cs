using Microsoft.AspNetCore.Mvc;
using SuperShop.Helpers;
using SuperShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        //Construtor
        //Vamos injetar o IUserHelper para poder ir buscar os respectivos metodos
        //Ctrl  + . em cima do userHelper e clicar em "Create and assign field userHelper"
        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        //Este action é só para mostrar a página do Login e tambem para dizer
        //se ja estivermos autenticado para sermos redirecionados para a Home nao mostrando a pagina de Login
        //Depois de elaborado este metodo temos que criar a respectiva View para o Login para isso
        //clicamos com o botao direito sobre Login() e fazemos Add View - Razor View - Add
        public IActionResult Login()
        {   //Se o utilizador quando fizer Login ja estiver autenticado é enviado para action Index do Home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            //Caso nao esteja Logado, fica na propria View 
            return View();
        }

        //Este action é para fazer o Login
        [HttpPost]
        //Recebe um LoginViewModel
        public async Task<IActionResult> Login (LoginViewModel model)
        {   //Se estiver tudo preenchido o modelo é valido
            if (ModelState.IsValid)
            {   //Vamo-nos tentar Logar
                var result = await _userHelper.LoginAsync(model);
                //Se conseguir fazer o Login
                if (result.Succeeded)
                {   //Ele logou e entrou atraves de um Url de retorno por ex: ele tenta aceder à
                    //pagina produtos mas só quem está logado tem acesso a essa pagina portanto´ao
                    //tentar entrar nessa pagina é lhe pedido que efetue Login caso efetue Login com
                    //sucesso é reemcaminhado para a pagina produtos
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return this.RedirectToAction("Index", "Home");
                }
            }
            //Se tentou logar e nao conseguiu passa logo para aqui, é apresentada
            //uma mensagem de erro e fica no mesmo sitio para que a pessoa nao tenha que
            //escrever de novo tudo e so tenha que alterar os dados incorretos
            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {   
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
