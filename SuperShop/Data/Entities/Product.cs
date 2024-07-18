using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    public class Product
    {   //Quando fazemos este Id nao precisamos de fazer mais nada, mas tem que ser só Id
        //ele atraves do nome Id deteta que é um id inteiro e automaticamente atribui
        //a chave primaria com este nome Id
        public int Id { get; set; }

        public string Name { get; set; }

        //Usando este DisplayFormat significa que vai formatar com duas casas decimais em modo
        //moeda mas quando tiver em modo de ediçao nao faz formato nenhum especifico deixando a pessoa escrever
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        //"Image" será o nome do campo vai aparecer na pagina da web, isto é só visual
        [Display(Name="Image")]
        public string ImageUrl { get; set; }


        [Display(Name = "Last Purchase")]
        public DateTime LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock {  get; set; }

    }
}
