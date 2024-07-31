using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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


        //Este action é só para mostrar a página do Register
        //Depois de elaborado este metodo temos que criar a respectiva View para o Register para isso
        //clicamos com o botao direito sobre Register() e fazemos Add View - Razor View - Add
        public IActionResult Register()
        {
            return View();
        }


        //Este action é para fazer o Register
        [HttpPost]
        //Recebe um RegisterNewUserViewModel
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {   //Se o modelo é válido
            if (ModelState.IsValid)
            {   //Ver se o user ja existe
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                //Se o user nao existe
                if (user == null)
                {   //Cria um user novo
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    //Adiciona o novo User
                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    //Se nao conseguiu criar o User
                    if (result != IdentityResult.Success)
                    {//Vai aparecer uma mensagem de erro
                        ModelState.AddModelError(string.Empty, "The User couldn't be created");
                        //Aqui passamos o model para os campos nao ficarem em branco e o utilizador poder corrigir
                        //aquilo que entender sem ter que preencher tudo novamente
                        return View(model);
                    }

                    //Se consegui criar o User é construido o LoginViewModel
                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    //Aqui está o Sigin, aqui vai tentar logar a partir do loginViewModel
                    var result2 = await _userHelper.LoginAsync(loginViewModel);
                    //Se consegue logar
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    //Se nao se consegue Logar
                    ModelState.AddModelError(string.Empty, "The User couldn't be logged");
                }                
            }
            return View(model);
        }

        //Este action é só para mostrar a página do ChangeUser
        //Depois de elaborado este metodo temos que criar a respectiva View para o ChangeUser para isso
        //clicamos com o botao direito sobre ChangeUser() e fazemos Add View - Razor View - Add
        public async Task<IActionResult> ChangeUser()
        {   //ver se o User existe
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            //Construir o modelo
            var model = new ChangeUserViewModel();
            //Se o suer existir passar os dados existentes sobre esse user
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return View(model);
        }



        //Este action é para fazer o ChangeUser
        [HttpPost]
        //Recebe um ChangeUserViewModel
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //ver se o User existe
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                //Se o suer existir passar os dados existentes sobre esse user
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    //Update mudando o FirstName e LastName
                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    //Se nao conseguir fazer o update manda uma mensagem de erro
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            return View(model);
        }

        //Este action é só para mostrar a página do ChangePassword
        //Depois de elaborado este metodo temos que criar a respectiva View para o ChangePassword para isso
        //clicamos com o botao direito sobre ChangePassword() e fazemos Add View - Razor View - Add
        public IActionResult ChangePassword()
        {
            return View();
        }

        //Este action é para fazer o ChangePassword
        [HttpPost]
        //Recebe um ChangePasswordViewModel
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {   //Se o modelo for válido
            if (ModelState.IsValid)
            {   //Verifica se o User existe e traz os dados
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                //Se o User existe
                if (user != null)
                {   //Modifica a Password
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {   //Redireciona para o ChangeUser
                        return this.RedirectToAction("ChangeUser");
                    }
                    //Se algo nao acontecer bem é mandado uma mensagem de erro
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                //Se nao encontrar o User manda esta mensagem e depois retorna para o View(model)
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }

    }
}
