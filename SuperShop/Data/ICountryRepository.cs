using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        // Método que devolve os países com as respetivas cidades
        IQueryable GetCountriesWithCities();

        // Método que através do ID devolve o objeto país com as cidades
        Task<Country> GetCountryWithCitiesAsync(int id);

        // Método que devolve o objeto cidade através do ID
        Task<City> GetCityAsync(int id);

        // Recebe o modelo e adiciona a cidade
        Task AddCityAsync(CityViewModel model);

        // Faz o update e devolve um inteiro que será o ID da cidade atualizada
        Task<int> UpdateCityAsync(City city);

        // Faz o delete e devolve um inteiro que será o ID da cidade removida
        Task<int> DeleteCityAsync(City city);
    }
}
