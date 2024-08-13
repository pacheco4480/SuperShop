using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    // Interface que define os métodos para manipulação de dados relacionados a países e cidades.
    public interface ICountryRepository : IGenericRepository<Country>
    {
        // Método que retorna uma consulta (IQueryable) contendo os países com suas cidades associadas.
        IQueryable GetCountriesWithCities();

        // Método assíncrono que, dado o ID de um país, retorna o objeto Country com suas cidades.
        Task<Country> GetCountryWithCitiesAsync(int id);

        // Método assíncrono que, dado o ID de uma cidade, retorna o objeto City correspondente.
        Task<City> GetCityAsync(int id);

        // Método assíncrono que recebe um modelo de vista (CityViewModel) e adiciona uma nova cidade.
        Task AddCityAsync(CityViewModel model);

        // Método assíncrono que atualiza uma cidade existente e retorna um inteiro representando o ID do país da cidade atualizada.
        Task<int> UpdateCityAsync(City city);

        // Método assíncrono que exclui uma cidade existente e retorna um inteiro representando o ID do país da cidade removida.
        Task<int> DeleteCityAsync(City city);

        // Método que retorna uma lista de SelectListItem para preencher um dropdown com países.
        IEnumerable<SelectListItem> GetComboCountries();

        // Método que, dado o ID de um país, retorna uma lista de SelectListItem para preencher um dropdown com as cidades desse país.
        IEnumerable<SelectListItem> GetComboCities(int countryId);

        // Método assíncrono que, dado um objeto City, retorna o objeto Country correspondente ao país dessa cidade.
        Task<Country> GetCountryAsync(City city);
    }
}
