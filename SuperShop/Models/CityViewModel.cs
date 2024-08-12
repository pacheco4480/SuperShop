using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class CityViewModel
    {
        // ID do país associado à cidade
        public int CountryId { get; set; }

        // ID da cidade
        public int CityId { get; set; }

        [Required]
        [Display(Name = "Cidade")] // Nome do campo a ser exibido
        [MaxLength(50, ErrorMessage = "O campo {0} pode conter {1} caracteres.")]
        public string Name { get; set; } // Nome da cidade
    }
}
