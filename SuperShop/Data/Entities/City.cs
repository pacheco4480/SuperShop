using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    // A classe City representa uma cidade e implementa a interface IEntity.
    public class City : IEntity
    {
        // Propriedade Id que representa o identificador único da cidade.
        public int Id { get; set; }

        // Propriedade Name que representa o nome da cidade.
        // O atributo [Required] indica que este campo é obrigatório.
        // O atributo [Display] define o nome a ser exibido na interface do utilizador.
        // O atributo [MaxLength] define o tamanho máximo permitido para o nome (50 caracteres).
        // ErrorMessage permite personalizar a mensagem de erro para quando o nome excede o tamanho máximo.
        [Required]
        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
