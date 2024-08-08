using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    // Define a classe OrderDetailTemp que implementa a interface IEntity
    public class OrderDetailTemp : IEntity
    {
        // Propriedade identificadora da entidade
        public int Id { get; set; }

        // Propriedade que representa o usuário associado ao detalhe do pedido.
        // A anotação [Required] indica que esta propriedade não pode ser nula.
        [Required]
        public User User { get; set; }

        // Propriedade que representa o produto associado ao detalhe do pedido.
        // A anotação [Required] indica que esta propriedade não pode ser nula.
        [Required]
        public Product Product { get; set; }

        // Propriedade que representa o preço do produto.
        // A anotação [DisplayFormat] formata o preço para mostrar como moeda com duas casas decimais.
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        // Propriedade que representa a quantidade do produto.
        // A anotação [DisplayFormat] formata a quantidade com duas casas decimais e separador de milhares.
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        // Propriedade calculada que retorna o valor total do produto (preço multiplicado pela quantidade).
        public decimal Value => Price * (decimal)Quantity;
    }
}
