using Microsoft.AspNetCore.Http;
using SuperShop.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{   //Na pratica isto é um Product igual mas que vamos acrescentar mais ficheiros
    public class ProductViewModel : Product
    {
        //Este ficheiro nao vai para a base de dados, se quisessemos que fosse teria que estar dentro do Product
        [Display(Name ="Image")]
        public IFormFile ImageFile { get; set; }
    }
}
