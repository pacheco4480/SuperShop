using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class RegisterNewUserViewModel
    {   //Vai ser obrigatorio preencher o FirstName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        //Vai ser obrigatorio preencher o LastName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        
        //Vai ser obrigatorio preencher o Username
        [Required]
        //O Utilizador terá que usar o email para se logar
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

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

        //Vai ser obrigatorio preencher a Password
        [Required]
        //A password tera que ter no minimo 6 caracteres este valor tem que
        //ser condizente com o tamanho da passowrd que definimos no ficheiro Startup.cs
        [MinLength(6)]
        public string Password { get; set; }

        //Confirmaçao, que serve para o utilizador por a password 2 vexes
        [Required]
        //Aqui vai confirmar que o valor que o utilizador poem no segundo campo é igual ao valor que defeniu para a Password
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
