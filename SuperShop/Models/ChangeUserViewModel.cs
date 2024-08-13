using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class ChangeUserViewModel
    {
        //Vai ser obrigatorio preencher o FirstName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //Vai ser obrigatorio preencher o LastName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Campo para o endereço do utilizador com comprimento máximo de 100 caracteres.
        // O atributo [MaxLength] define a mensagem de erro se o limite for excedido.
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string Address { get; set; }

        // Campo para o número de telefone do utilizador com comprimento máximo de 20 caracteres.
        // O atributo [MaxLength] define a mensagem de erro se o limite for excedido.
        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string PhoneNumber { get; set; }

        // Campo obrigatório para o ID da cidade, representado por um valor inteiro.
        // O atributo [Range] garante que o utilizador selecione uma cidade válida.
        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        public int CityId { get; set; }

        // Lista de itens de seleção para as cidades disponíveis.
        // Utilizado para preencher um dropdown na interface de utilizador.
        public IEnumerable<SelectListItem> Cities { get; set; }

        // Campo obrigatório para o ID do país, representado por um valor inteiro.
        // O atributo [Range] garante que o utilizador selecione um país válido.
        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }

        // Lista de itens de seleção para os países disponíveis.
        // Utilizado para preencher um dropdown na interface de utilizador.
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
