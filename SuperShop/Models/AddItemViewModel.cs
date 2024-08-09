using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class AddItemViewModel
    {   
        //Isto será mostrado numa combobox
        [Display(Name ="Product")]
        //Ira ser mostrada a combobox e a pessoa terá que selecionar um produto
        [Range(1,int.MaxValue, ErrorMessage ="You must select a product.")]
        public int ProductId { get; set; }

        //Ira ser mostrada a combobox e a pessoa terá que selecionar a quantidade
        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public double Quantity { get; set; }

        //Lista com todos os produtos
        //Na pratica estamos a criar uma Lista de items de listas
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
