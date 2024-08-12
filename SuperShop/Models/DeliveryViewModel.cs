using System.ComponentModel.DataAnnotations;
using System;

namespace SuperShop.Models
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }

        // Propriedade que representa a data de entrega do pedido.
        // A anotação [Display] define o nome exibido para a propriedade na interface de utilizador.
        // A anotação [DisplayFormat] formata a data para o formato 'yyyy/MM/dd hh:mm tt'.
        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime DeliveryDate { get; set; }
    }
}
