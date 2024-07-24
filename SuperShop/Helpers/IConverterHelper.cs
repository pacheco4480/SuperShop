using SuperShop.Data.Entities;
using SuperShop.Models;

namespace SuperShop.Helpers
{
    public interface IConverterHelper
    {
        //Recebe um ProductViewModel e converte em Product
        Product ToProduct(ProductViewModel model, string path, bool isNew);

        //Recebe um Product e converte em ProductViewModel
        ProductViewModel ToProductViewModel(Product product);
    }
}
