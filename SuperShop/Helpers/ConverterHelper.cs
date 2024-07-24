using SuperShop.Data.Entities;
using SuperShop.Models;
using System.IO;

namespace SuperShop.Helpers
{    //Ctrl  + . em cima do IConverterHelper para implementar a interface e fazemos "implement interface"
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel model, string path, bool isNew)
        {
            return new Product
            {   //Aqui no Id se o valor vier true significa que é novo entao o Id vai a 0 e depois coloca la ele o Id na
                //base de dados se nao for novo vem atraves do Edit entao mete lá o Id diretamente
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }
    }
}
