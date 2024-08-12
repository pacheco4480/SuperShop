using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    // A classe Country representa um país e implementa a interface IEntity.
    public class Country : IEntity
    {
        // Propriedade Id que representa o identificador único do país.
        public int Id { get; set; }

        // Propriedade Name que representa o nome do país.
        // O atributo [Required] indica que este campo é obrigatório.
        // O atributo [MaxLength] define o tamanho máximo permitido para o nome (50 caracteres).
        // ErrorMessage permite personalizar a mensagem de erro para quando o nome excede o tamanho máximo.
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }

        // Propriedade Cities que representa uma coleção de cidades associadas ao país.
        // ICollection<City> é utilizada para representar uma coleção de objetos do tipo City.
        public ICollection<City> Cities { get; set; }

        // Propriedade de exibição que calcula e retorna o número de cidades associadas ao país.
        // [Display] personaliza o nome a ser exibido para esta propriedade na interface do utilizador.
        // Utiliza uma expressão condicional para retornar 0 se a coleção Cities for nula, caso contrário, retorna o número de elementos na coleção.
        [Display(Name = "Number of cities")]
        public int NumberCities => Cities == null ? 0 : Cities.Count;
    }
}
