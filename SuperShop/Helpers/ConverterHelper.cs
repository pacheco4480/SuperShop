using SuperShop.Data.Entities;
using SuperShop.Models;
using System;
using System.IO;

namespace SuperShop.Helpers
{
    // Implementa a interface IConverterHelper. Esta interface define métodos para converter entre Product e ProductViewModel.
    public class ConverterHelper : IConverterHelper
    {
        //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
        // Implementa o método que converte um ProductViewModel num Product.
        // Recebe um modelo de visualização (ProductViewModel), um caminho para a imagem (path) e um booleano (isNew) indicando se é um novo produto.
        //public Product ToProduct(ProductViewModel model, string path, bool isNew)

        //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
        public Product ToProduct(ProductViewModel model, Guid imageId, bool isNew)
        {
            // Cria e retorna um novo objeto Product com os valores do ProductViewModel.
            return new Product
            {
                // Se for um novo produto, o Id será 0; caso contrário, usa o Id do modelo recebido.
                Id = isNew ? 0 : model.Id,
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                //// A URL da imagem será o caminho fornecido.
                //ImageUrl = path,
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                ImageId = imageId,
                // Os outros campos são copiados diretamente do modelo de visualização.
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        // Implementa o método que converte um Product em um ProductViewModel.
        public ProductViewModel ToProductViewModel(Product product)
        {
            // Cria e retorna um novo objeto ProductViewModel com os valores do Product.
            return new ProductViewModel
            {

                // Copia todos os campos diretamente do objeto Product recebido.
                Id = product.Id,
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                //ImageUrl = product.ImageUrl,
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                ImageId = product.ImageId,
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