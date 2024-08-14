using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly ICountryRepository _countryRepository;

        //Construtor
        //Vamos injetar o IUserHelper para poder ir buscar os respectivos metodos
        //Ctrl  + . em cima do userHelper e clicar em "Create and assign field userHelper"
        //Ctrl  + . em cima do countryRepository e clicar em "Create and assign field countryRepository"
        //Ctrl  + . em cima do configuration e clicar em "Create and assign field configuration"
        //Ctrl  + . em cima do mailHelper e clicar em "Create and assign field mailHelper"
        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            ICountryRepository countryRepository)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
            _countryRepository = countryRepository;
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


        // Esta ação é responsável por exibir a página de registro de um novo usuário.
        // Depois de implementar este método, é necessário criar a respectiva View para o Register.
        // Para isso, clique com o botão direito sobre Register() e selecione Add View - Razor View - Add.
        public IActionResult Register()
        {   //Combobox
            // Cria uma nova instância do modelo RegisterNewUserViewModel, que será passado para a view.
            // Este modelo inclui listas para preencher os comboboxes de países e cidades.
            var model = new RegisterNewUserViewModel
            {
                // Obtém uma lista de países para preencher o combobox de países.
                Countries = _countryRepository.GetComboCountries(),

                // Inicializa a lista de cidades com o combobox vazio, pois nenhum país foi selecionado inicialmente.
                // 0 é usado como valor padrão até que um país seja selecionado.
                Cities = _countryRepository.GetComboCities(0)
            };

            // Retorna a view de registo com o modelo criado, permitindo ao utilizador preencher os dados necessários.
            return View(model);
        }



        // Esta ação é responsável por processar o registo de um novo utilizador no sistema.
        // O método utiliza a anotação [HttpPost] para indicar que ele lida com requisições HTTP POST,
        // que são feitas quando o formulário de registo é submetido.
        [HttpPost]
        // Recebe um objeto do tipo RegisterNewUserViewModel que contém os dados do utilizador a ser registado.
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            // Verifica se o modelo é válido, ou seja, se todos os campos obrigatórios estão preenchidos corretamente.
            if (ModelState.IsValid)
            {
                // Verifica se já existe um utilizador com o email fornecido (Username).
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                // Se o utilizador não existir, procede com a criação de um novo utilizador.
                if (user == null)
                {
                    // Obtém a cidade selecionada pelo utilizador através do repositório de países.
                    // A cidade é verificada antes de criar o utilizador para garantir que ela existe.
                    var city = await _countryRepository.GetCityAsync(model.CityId);

                    // Cria uma nova instância do utilizador com os dados fornecidos pelo modelo.
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username, // O email também é usado como nome de utilizador.
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        CityId = model.CityId,
                        City = city // Associa a cidade obtida ao utilizador.
                    };

                    // Tenta adicionar o novo utilizador ao sistema com a password fornecida.
                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    // Verifica se o utilizador foi criado com sucesso.
                    if (result != IdentityResult.Success)
                    {
                        // Se houver um erro ao criar o utilizador, adiciona uma mensagem de erro ao modelo.
                        ModelState.AddModelError(string.Empty, "O utilizador não pôde ser criado.");

                        // Retorna a vista com o modelo atual para que o utilizador possa corrigir os erros.
                        return View(model);
                    }

                    // Gera um token de confirmação de email para o novo utilizador.
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    // Cria um link de confirmação de email gerado na action ConfirmEmail será enviado para o utilizador.
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id, // O ID do utilizador para a confirmação.
                        token = myToken // O token gerado para a confirmação do email.
                    }, protocol: HttpContext.Request.Scheme);

                    // Envia um email ao utilizador com o link de confirmação.
                    //Quando a pessoa carregar no tokenLink vai Confirmar(ConfirmEmail) o email e mandar dois parametros userid e token
                    Response response = _mailHelper.SendEmail(model.Username, "Confirmação de Email",
                        $"<h1>Confirmação de Email</h1>" +
                        $"Para confirmar o seu email, " +
                        $"por favor clique neste link:</br></br><a href=\"{tokenLink}\">Confirmar Email</a>");

                    // Verifica se o email foi enviado com sucesso.
                    if (response.IsSuccess)
                    {
                        // Se o email foi enviado com sucesso, adiciona uma mensagem de sucesso ao ViewBag.
                        ViewBag.Message = "As instruções para confirmar o seu utilizador foram enviadas para o email.";

                        // Retorna a vista com o modelo atual, mantendo os dados preenchidos.
                        return View(model);
                    }

                    // Se o envio do email falhar, adiciona uma mensagem de erro ao modelo.
                    ModelState.AddModelError(string.Empty, "O utilizador não pôde ser iniciado sessão.");
                }
            }

            // Se o modelo não for válido ou se houver erros, retorna a vista com o modelo atual.
            return View(model);
        }



        // Esta ação é responsável por exibir a página de alteração de dados do utilizador (ChangeUser).
        // Após desenvolver este método, é necessário criar a respetiva View para o ChangeUser.
        // Para isso, clica-se com o botão direito sobre ChangeUser(), seleciona-se Add View, depois Razor View, e por fim Add.
        public async Task<IActionResult> ChangeUser()
        {
            // Obtém o utilizador atual a partir do email armazenado na identidade do utilizador logado.
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            // Cria um novo modelo de vista para alteração de dados do utilizador.
            var model = new ChangeUserViewModel();

            // Se o utilizador existir, preenche os dados existentes no modelo para exibição na vista.
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;

                //Combobox - Obtém a cidade do utilizador e preenche as comboboxes de cidades e países.
                var city = await _countryRepository.GetCityAsync(user.CityId);

                if (city != null)
                {
                    // Se a cidade existir, obtém o país correspondente.
                    var country = await _countryRepository.GetCountryAsync(city);

                    if (country != null)
                    {
                        // Preenche os campos do modelo com os dados do país e das cidades correspondentes.
                        model.CountryId = country.Id;
                        model.Cities = _countryRepository.GetComboCities(country.Id);
                        model.Countries = _countryRepository.GetComboCountries();
                        model.CityId = user.CityId;
                    }
                }
            }

            // Caso não tenha sido possível carregar as cidades anteriormente, preenche as comboboxes com listas padrão.
            model.Cities = _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();

            // Retorna a vista com o modelo preenchido, permitindo que o utilizador visualize e altere os seus dados.
            return View(model);
        }




        // Este método processa a submissão do formulário de alteração de dados do utilizador (ChangeUser).
        [HttpPost]
        // O método recebe um objeto ChangeUserViewModel que contém os dados preenchidos pelo utilizador na vista.
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            // Verifica se os dados enviados no modelo são válidos.
            if (ModelState.IsValid)
            {
                // Obtém o utilizador atual a partir do email armazenado na identidade do utilizador logado.
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                // Se o utilizador existir, atualiza os dados com as informações fornecidas no modelo.
                if (user != null)
                {
                    // Obtém a cidade selecionada pelo utilizador para associá-la ao utilizador.
                    var city = await _countryRepository.GetCityAsync(model.CityId);

                    // Atualiza os dados do utilizador com as informações do modelo.
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.CityId = model.CityId;
                    user.City = city;

                    // Tenta atualizar os dados do utilizador no repositório.
                    var response = await _userHelper.UpdateUserAsync(user);

                    // Se a atualização for bem-sucedida, exibe uma mensagem de confirmação ao utilizador.
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "Utilizador atualizado com sucesso!";
                    }
                    // Se a atualização falhar, exibe a primeira mensagem de erro retornada pelo repositório.
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            // Retorna a vista com o modelo atual, permitindo que o utilizador corrija qualquer erro nos dados enviados.
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

        // Esta action é responsável por criar um token JWT para a API para autenticar um utilizador.
        // Recebe um modelo de login contendo o nome de utilizador e a password através de uma requisição POST.
        // Se o modelo for válido e a password for correta, o método gera um token JWT que pode ser usado para autenticação em chamadas subsequentes à API.
        // O token inclui informações sobre o utilizador e expira após um período definido (15 dias neste caso).
        // Retorna um resultado HTTP 201 Created com o token e a data de expiração se tudo estiver correto,
        // ou um erro HTTP 400 Bad Request se o modelo for inválido ou o utilizador não for encontrado.
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            // Verifica se o modelo recebido é válido
            if (this.ModelState.IsValid)
            {
                // Obtém o utilizador com base no email fornecido no modelo de login
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                // Se o utilizador for encontrado
                if (user != null)
                {
                    // Valida a password fornecida com a do utilizador
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    // Se a password estiver correta
                    if (result.Succeeded)
                    {
                        // Define as claims (declarações) que serão incluídas no token JWT
                        var claims = new[]
                        {
                    // A claim "sub" representa o subject (utilizador) do token, neste caso o email do utilizador
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    
                    // A claim "jti" é um identificador único para o token, geralmente usado para prevenir a reutilização
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        // Cria uma chave simétrica para assinar o token, usando uma chave configurada nas configurações da aplicação
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

                        // Define as credenciais de assinatura usando o algoritmo HMAC-SHA256
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Cria o token JWT com as claims, as credenciais de assinatura, o emissor, o público e a data de expiração
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"], // Emissor do token
                            _configuration["Tokens:Audience"], // Público destinatário do token
                            claims, // Claims que definem o conteúdo do token
                            expires: DateTime.UtcNow.AddDays(15), // Define a expiração do token para 15 dias a partir da criação
                            signingCredentials: credentials); // Credenciais para assinar o token

                        // Cria um objeto para devolver como resultado, contendo o token gerado e a sua data de expiração
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token), // Converte o token para uma string
                            expiration = token.ValidTo // Data e hora de expiração do token
                        };

                        // Retorna uma resposta HTTP 201 Created com o token e a data de expiração no corpo da resposta
                        return this.Created(string.Empty, results);
                    }
                }
            }

            // Se o modelo não for válido ou o utilizador não for encontrado, retorna um BadRequest (400) com as mensagens de erro
            return BadRequest("Dados inválidos ou utilizador não encontrado.");
        }

        // Este método é responsável por confirmar o email de um utilizador usando um token de confirmação.
        // A ação é chamada quando o utilizador clica no link de confirmação enviado por email.
        //Depois de elaborado esta action temos que criar a respectiva View para o ConfirmEmail para isso
        //clicamos com o botao direito sobre ConfirmEmail() e fazemos Add View - Razor View - Add
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // Verifica se o ID do utilizador ou o token estão vazios ou nulos.
            // Se algum dos parâmetros estiver ausente, retorna um resultado NotFound (404).
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound(); // Retorna uma página de erro 404 se os parâmetros forem inválidos.
            }

            // Obtém o utilizador com base no ID fornecido.
            var user = await _userHelper.GetUserByIdAsync(userId);

            // Verifica se o utilizador existe.
            // Se o utilizador não for encontrado, retorna um resultado NotFound (404).
            if (user == null)
            {
                return NotFound(); // Retorna uma página de erro 404 se o utilizador não for encontrado.
            }

            // Tenta confirmar o email do utilizador usando o token fornecido.
            var result = await _userHelper.ConfirmEmailAsync(user, token);

            // Verifica se a confirmação do email foi bem-sucedida.
            if (!result.Succeeded)
            {
                // Se a confirmação falhar, pode-se adicionar lógica para lidar com erros,
                // como adicionar uma mensagem de erro ao modelo ou redirecionar para uma página de erro.
                // Neste momento, não há lógica adicional, mas pode-se adicionar uma mensagem ou redirecionar conforme necessário.
            }

            // Retorna uma vista indicando o resultado da confirmação do email.
            // Pode-se personalizar a vista para mostrar uma mensagem de sucesso ou erro conforme necessário.
            return View(); // Retorna a vista padrão, que pode ser personalizada para mostrar o resultado da confirmação.
        }



        //Este action é só para mostrar a página do NotAuthorized
        //Depois de elaborado este metodo temos que criar a respectiva View para o NotAuthorized para isso
        //clicamos com o botao direito sobre NotAuthorized() e fazemos Add View - Razor View - Add
        public IActionResult NotAuthorized()
        {
            return View();
        }

        // Combobox - Esta ação serve para atualizar a lista de cidades numa combobox quando um país é selecionado
        // noutra combobox. É uma ação auxiliar utilizada para preencher dinamicamente as cidades
        // correspondentes ao país selecionado, sem necessidade de recarregar toda a página.
        // Indica que o método responde a requisições HTTP POST.
        [HttpPost]
        // Define a rota para a ação, permitindo que ela seja acessada através da URL especificada.
        [Route("Account/GetCitiesAsync")]
        // Este método retorna um resultado JSON contendo as cidades associadas ao país selecionado.
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            // Obtém o país com as suas cidades utilizando o repositório de países.
            // O método GetCountryWithCitiesAsync retorna um objeto país que contém uma lista de cidades.
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);

            // Retorna um resultado JSON ordenado com as cidades do país.
            // As cidades são ordenadas por nome para facilitar a seleção na interface do utilizador.
            return Json(country.Cities.OrderBy(c => c.Name));
        }


    }
}
