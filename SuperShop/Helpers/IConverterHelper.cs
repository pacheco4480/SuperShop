using SuperShop.Data.Entities;
using SuperShop.Models;
using System;

namespace SuperShop.Helpers
{
    public interface IConverterHelper
    {   //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
        //Recebe um ProductViewModel e converte em Product
        //Product ToProduct(ProductViewModel model, string path, bool isNew);
        //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
        Product ToProduct(ProductViewModel model, Guid imageId, bool isNew);

        //Recebe um Product e converte em ProductViewModel
        ProductViewModel ToProductViewModel(Product product);
    }
}
