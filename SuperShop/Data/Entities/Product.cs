using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    //IEntity é a Interface que estamos a implementar na class Product
    public class Product : IEntity
    {   //Quando fazemos este Id nao precisamos de fazer mais nada, mas tem que ser só Id
        //ele atraves do nome Id deteta que é um id inteiro e automaticamente atribui
        //a chave primaria com este nome Id
        public int Id { get; set; }

        //Este Required faz com que seja obrigatorio preencher o Nome do produto
        [Required]
        //O nome do produto só poderá ter no máximo 50 caracteres
        //{0} é o primeiro parametro que neste caso é o Name, {1} é o segundo parametro que vai buscar o numero 50
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }

        //Usando este DisplayFormat significa que vai formatar com duas casas decimais em modo
        //moeda mas quando tiver em modo de ediçao nao faz formato nenhum especifico deixando a pessoa escrever
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
        //"Image" será o nome do campo vai aparecer na pagina da web, isto é só visual
        //Substituir esta propriedade pela propriedade de baixo pois agora nao vamos obter as imagens atraves do URl's 
        //agora é atraves dos containers
        //[Display(Name="Image")]
        //public string ImageUrl { get; set; }

        //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }


        [Display(Name = "Last Purchase")]
        //Pondo o "?" em frente do DateTime torna opcional a inserçao da data
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        //Esta propriedade mostra o User que inseriu o produto
        public User User { get; set; }

        //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
        //Como substituimos em cima public string ImageUrl { get; set; } deixamos a usar esta string passando a
        //usar o que está em baixo
        //public string ImageFullPath
        //{
        //    get
        //    {   //Se estiver vazio retorna null
        //        if (string.IsNullOrEmpty(ImageUrl))
        //        {
        //            return null;
        //        }
        //        //Caso nao esteja vazio, este é o endereço do nosso site
        //        return $"https://supershop88.azurewebsites.net{ImageUrl.Substring(1)}";
        //    }
        //}

        //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
        //Se nao tiver imagem, ele vai buscar a imagem que temos por defeito na pasta imagens
        //Se tiver imagens para aparesentar significa que o Guid nao é empty e vai entao buscar as imagens ao blob
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://supershop88.azurewebsites.net/images/noimage.png"
            //Caso tenha imagem
            : $"https://supershopsi88.blob.core.windows.net/products/{ImageId}";
    }
}