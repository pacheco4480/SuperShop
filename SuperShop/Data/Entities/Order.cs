using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;

namespace SuperShop.Data.Entities
{
    // Define a classe Order que implementa a interface IEntity
    public class Order : IEntity
    {
        // Propriedade identificadora da entidade
        public int Id { get; set; }

        // Propriedade que representa a data do pedido.
        // A anotação [Required] indica que esta propriedade não pode ser nula.
        // A anotação [Display] define o nome exibido para a propriedade na interface de utilizador.
        // A anotação [DisplayFormat] formata a data para o formato 'yyyy/MM/dd hh:mm tt'.
        [Required]
        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }

        // Propriedade que representa a data de entrega do pedido.
        // A anotação [Required] indica que esta propriedade não pode ser nula.
        // A anotação [Display] define o nome exibido para a propriedade na interface de utilizador.
        // A anotação [DisplayFormat] formata a data para o formato 'yyyy/MM/dd hh:mm tt'.
        [Required]
        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime DeliveryDate { get; set; }

        // Propriedade que representa o usuário associado ao pedido.
        // A anotação [Required] indica que esta propriedade não pode ser nula.
        [Required]
        public User User { get; set; }

        // Propriedade que representa a coleção de detalhes do pedido.
        // Esta propriedade estabelece uma relação de um para muitos com a tabela OrderDetails,
        // sendo que, na prática, será uma lista de objetos OrderDetail.
        public IEnumerable<OrderDetail> Items { get; set; }

        // Propriedade calculada que retorna a quantidade total dos itens no pedido.
        // A anotação [DisplayFormat] formata a quantidade com duas casas decimais e separador de milhares.
        // Se Items for nulo, a quantidade total será 0.
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Items == null ? 0 : Items.Sum(i => i.Quantity);

        // Propriedade calculada que retorna o valor total dos itens no pedido.
        // A anotação [DisplayFormat] formata o valor total como moeda com duas casas decimais.
        // Se Items for nulo, o valor total será 0.
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Value);
    }
}
