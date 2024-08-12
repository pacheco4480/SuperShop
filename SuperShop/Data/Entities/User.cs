using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    // A classe User herda de IdentityUser, que já contém propriedades predefinidas para autenticação e autorização.
    // Vamos adicionar propriedades adicionais específicas para a nossa aplicação.
    public class User : IdentityUser
    {
        // Propriedade para armazenar o primeiro nome do utilizador
        // Define um comprimento máximo de 50 caracteres e uma mensagem de erro caso o limite seja excedido
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string FirstName { get; set; }

        // Propriedade para armazenar o último nome do utilizador
        // Define um comprimento máximo de 50 caracteres e uma mensagem de erro caso o limite seja excedido
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string LastName { get; set; }

        // Propriedade para armazenar o endereço do utilizador
        // Define um comprimento máximo de 100 caracteres e uma mensagem de erro caso o limite seja excedido
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string Address { get; set; }

        // Propriedade que armazena o ID da cidade do utilizador
        // Isso é usado para relacionar o utilizador com a sua cidade
        public int CityId { get; set; }

        // Objeto de navegação que permite acesso aos dados completos da cidade
        // Facilita o acesso a outras informações da cidade associada ao utilizador
        public City City { get; set; }

        // Propriedade que retorna o nome completo do utilizador
        // Combina FirstName e LastName para uma exibição completa do nome
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
