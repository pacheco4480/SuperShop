using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data.Entities;
using SuperShop.Data;
using SuperShop.Models;
using System.Threading.Tasks;
using System;

namespace SuperShop.Controllers
{
    // O controlador está protegido por autorização, apenas utilizadores com o papel de "Admin" podem aceder a este controlador.
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        //private readonly IFlashMessage _flashMessage;

        // O construtor do controlador recebe duas dependências: ICountryRepository para acesso a dados sobre países e cidades,
        // e IFlashMessage para mensagens de feedback para o utilizador.
        //Ctrl  + . em cima do countryRepository e clicar em "Create and assign field countryRepository"
        public CountriesController(
            ICountryRepository countryRepository
            /*IFlashMessage flashMessage*/)
        {
            _countryRepository = countryRepository;
            //_flashMessage = flashMessage;
        }

        // Ação para excluir uma cidade com base no Id fornecido.
        // Recebe um id opcional. Se o id não for fornecido ou a cidade não for encontrada, retorna uma página "NotFound".
        // Caso contrário, exclui a cidade e redireciona para a ação "Details" do país ao qual a cidade pertencia.
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna "NotFound" se o Id não for fornecido.
            }

            var city = await _countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound(); // Retorna "NotFound" se a cidade não for encontrada.
            }

            var countryId = await _countryRepository.DeleteCityAsync(city);
            return RedirectToAction("Details", new { id = countryId }); // Redireciona para a página de detalhes do país.
        }

        // Ação para exibir o formulário de edição de uma cidade.
        // Recebe um id opcional. Se o id não for fornecido ou a cidade não for encontrada, retorna uma página "NotFound".
        // Caso contrário, exibe o formulário de edição da cidade.
        //Depois de elaborada esta action temos que criar a respectiva View para o EditCity para isso
        //clicamos com o botao direito sobre EditCity() e fazemos Add View - Razor View - Add - Add
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna "NotFound" se o Id não for fornecido.
            }

            var city = await _countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound(); // Retorna "NotFound" se a cidade não for encontrada.
            }

            return View(city); // Exibe o formulário de edição da cidade.
        }

        // Ação POST para atualizar uma cidade com base no modelo fornecido.
        // Se o modelo for válido, atualiza a cidade e redireciona para a página de detalhes do país ao qual a cidade pertence.
        // Se o modelo não for válido, exibe novamente o formulário de edição com os erros de validação.
        [HttpPost]
        public async Task<IActionResult> EditCity(City city)
        {
            if (ModelState.IsValid)
            {
                var countryId = await _countryRepository.UpdateCityAsync(city);
                if (countryId != 0)
                {
                    return RedirectToAction("Details", new { id = countryId }); // Redireciona para a página de detalhes do país.
                }
            }

            return View(city); // Exibe o formulário de edição da cidade se houver erros de validação.
        }

        // Ação para exibir o formulário para adicionar uma nova cidade a um país.
        // Recebe um id opcional do país. Se o id não for fornecido ou o país não for encontrado, retorna uma página "NotFound".
        // Caso contrário, exibe o formulário de adição de cidade.
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna "NotFound" se o Id não for fornecido.
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound(); // Retorna "NotFound" se o país não for encontrado.
            }

            var model = new CityViewModel { CountryId = country.Id };
            return View(model); // Exibe o formulário de adição de cidade.
        }

        // Ação POST para adicionar uma nova cidade com base no modelo fornecido.
        // Se o modelo for válido, adiciona a nova cidade e redireciona para a página de detalhes do país ao qual a cidade foi adicionada.
        // Se o modelo não for válido, exibe novamente o formulário de adição com os erros de validação.
        //Depois de elaborada esta action temos que criar a respectiva View para o AddCity para isso
        //clicamos com o botao direito sobre AddCity() e fazemos Add View - Razor View - Add - Add
        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _countryRepository.AddCityAsync(model);
                return RedirectToAction("Details", new { id = model.CountryId }); // Redireciona para a página de detalhes do país.
            }

            return View(model); // Exibe o formulário de adição de cidade se houver erros de validação.
        }

        // Ação para listar todos os países com suas cidades.
        //Depois de elaborada esta action temos que criar a respectiva View para o Index para isso
        //clicamos com o botao direito sobre Index() e fazemos Add View - Razor View - Add - Add
        public IActionResult Index()
        {
            return View(_countryRepository.GetCountriesWithCities()); // Exibe a lista de países com suas cidades.
        }

        // Ação para exibir os detalhes de um país com base no Id fornecido.
        // Recebe um id opcional. Se o id não for fornecido ou o país não for encontrado, retorna uma página "NotFound".
        // Caso contrário, exibe os detalhes do país.
        //Depois de elaborada esta action temos que criar a respectiva View para o Details para isso
        //clicamos com o botao direito sobre Details() e fazemos Add View - Razor View - Add - Add
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna "NotFound" se o Id não for fornecido.
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(id.Value);
            if (country == null)
            {
                return NotFound(); // Retorna "NotFound" se o país não for encontrado.
            }

            return View(country); // Exibe os detalhes do país.
        }

        // Ação para exibir o formulário de criação de um novo país.
        //Depois de elaborada esta action temos que criar a respectiva View para o Create para isso
        //clicamos com o botao direito sobre Create() e fazemos Add View - Razor View - Add - Add
        public IActionResult Create()
        {
            return View(); // Exibe o formulário de criação de um novo país.
        }

        // Ação POST para criar um novo país com base no modelo fornecido.
        // Se o modelo for válido, cria o novo país e redireciona para a lista de países.
        // Se o modelo não for válido ou ocorrer uma exceção (por exemplo, se o país já existir), exibe novamente o formulário de criação com erros.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                await _countryRepository.CreateAsync(country);
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de países.
            }

            return View(country); // Exibe o formulário de criação se o modelo não for válido.
        }

        // Ação para exibir o formulário de edição de um país com base no Id fornecido.
        // Recebe um id opcional. Se o id não for fornecido ou o país não for encontrado, retorna uma página "NotFound".
        // Caso contrário, exibe o formulário de edição do país.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna "NotFound" se o Id não for fornecido.
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound(); // Retorna "NotFound" se o país não for encontrado.
            }
            return View(country); // Exibe o formulário de edição do país.
        }

        // Ação POST para atualizar um país com base no modelo fornecido.
        // Se o modelo for válido, atualiza o país e redireciona para a lista de países.
        // Se o modelo não for válido, exibe novamente o formulário de edição com erros de validação.
        //Depois de elaborada esta action temos que criar a respectiva View para o Edit para isso
        //clicamos com o botao direito sobre Edit() e fazemos Add View - Razor View - Add - Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                await _countryRepository.UpdateAsync(country);
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de países.
            }

            return View(country); // Exibe o formulário de edição se houver erros de validação.
        }

        // Ação para excluir um país com base no Id fornecido.
        // Recebe um id opcional. Se o id não for fornecido ou o país não for encontrado, retorna uma página "NotFound".
        // Caso contrário, exclui o país e redireciona para a lista de países.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna "NotFound" se o Id não for fornecido.
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound(); // Retorna "NotFound" se o país não for encontrado.
            }

            await _countryRepository.DeleteAsync(country);
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de países.
        }
    }
}
