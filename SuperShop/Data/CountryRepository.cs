using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    // A classe CountryRepository implementa a interface ICountryRepository
    // e herda de GenericRepository<Country>. Isto garante que a classe 
    // fornece a implementação para todos os métodos definidos na interface ICountryRepository
    // e herda os métodos genéricos para manipular entidades Country.
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        // Construtor da classe CountryRepository que aceita um DataContext como parâmetro.
        // Este construtor passa o contexto para a classe base GenericRepository
        // e inicializa o contexto na classe CountryRepository.
        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        // Cria uma nova cidade ao país especificado pelo CountryId tendo como principio que ja existe o Pais
        // Recebe um CityViewModel que contém informações sobre a cidade a ser adicionada.
        public async Task AddCityAsync(CityViewModel model)
        {
            // Obtém o país com as suas cidades usando o CountryId do modelo.
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                // Se o país não for encontrado, sai do método sem fazer alterações.
                return;
            }
            //Caso haja cidades
            // Adiciona a nova cidade à lista de cidades do país.
            country.Cities.Add(new City { Name = model.Name });
            // Atualiza o país no contexto.
            _context.Countries.Update(country);
            // Salva as alterações na base de dados de forma assíncrona.
            await _context.SaveChangesAsync();
        }

        // Remove uma cidade da base de dados.
        // Recebe a cidade a ser removida e retorna o Id do país da qual a cidade foi removida.
        public async Task<int> DeleteCityAsync(City city)
        {
            // Procura o país que contém a cidade especificada pelo Id da cidade.
            var country = await _context.Countries
                //Vai buscar todas as cidades daquele Country
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();
            //Se nao encontrar a cidade
            if (country == null)
            {
                // Se o país não for encontrado, retorna 0 indicando que a cidade não foi removida.
                return 0;
            }
            //Se encontrar a cidade
            // Remove a cidade do contexto.
            _context.Cities.Remove(city);
            // Salva as alterações na base de dados de forma assíncrona.
            await _context.SaveChangesAsync();
            // Retorna o Id do país da qual a cidade foi removida.
            return country.Id;
        }

        // Obtém todos os países com as suas cidades associadas.
        // Retorna uma consulta IQueryable que inclui os países e as suas cidades.
        public IQueryable GetCountriesWithCities()
        {
            return _context.Countries
                .Include(c => c.Cities) // Inclui a lista de cidades para cada país.
                .OrderBy(c => c.Name); // Ordena os países pelo nome.
        }

        // Obtém um país especifico com as suas cidades associadas com base no Id do país.
        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await _context.Countries
                .Include(c => c.Cities) // Inclui as cidades do país.
                .Where(c => c.Id == id) // Filtra o país pelo Id.
                .FirstOrDefaultAsync(); // Retorna o país ou null se não encontrado.
        }

        // Atualiza uma cidade na base de dados e retorna o Id do país ao qual a cidade pertence.
        public async Task<int> UpdateCityAsync(City city)
        {
            // Procura o país que contém a cidade especificada pelo Id da cidade.
            var country = await _context.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                // Se o país não for encontrado, retorna 0 indicando que a cidade não foi atualizada.
                return 0;
            }

            // Atualiza a cidade no contexto.
            _context.Cities.Update(city);
            // Salva as alterações na base de dados de forma assíncrona.
            await _context.SaveChangesAsync();
            // Retorna o Id do país ao qual a cidade pertence.
            return country.Id;
        }

        // Obtém uma cidade com base no Id fornecido.
        public async Task<City> GetCityAsync(int id)
        {
            return await _context.Cities.FindAsync(id);
        }

        // Obtém o país ao qual uma cidade pertence com base na cidade fornecida.
        public async Task<Country> GetCountryAsync(City city)
        {
            return await _context.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();
        }

        // Obtém uma lista de países formatada para ser usada em um dropdown (lista suspensa).
        // Retorna uma lista de SelectListItem com países ordenados por nome.
        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name, // O texto exibido no dropdown.
                Value = c.Id.ToString() // O valor associado ao item do dropdown.
            }).OrderBy(l => l.Text).ToList(); // Ordena os países por nome.

            // Adiciona uma opção de seleção inicial ao dropdown.
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country...)", // Texto para a opção de seleção.
                Value = "0" // Valor da opção de seleção.
            });

            return list;
        }

        // Obtém uma lista de cidades formatada para ser usada em um dropdown baseado no Id do país.
        // Retorna uma lista de SelectListItem com cidades ordenadas por nome.
        public IEnumerable<SelectListItem> GetComboCities(int countryId)
        {
            var country = _context.Countries.Find(countryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = _context.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name, // O texto exibido no dropdown.
                    Value = c.Id.ToString() // O valor associado ao item do dropdown.
                }).OrderBy(l => l.Text).ToList(); // Ordena as cidades por nome.

                // Adiciona uma opção de seleção inicial ao dropdown.
                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a city...)", // Texto para a opção de seleção.
                    Value = "0" // Valor da opção de seleção.
                });
            }

            return list;
        }
    }
}
