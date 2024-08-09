using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using SuperShop.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SuperShop.Data
{
    // A interface IProductRepository vai herdar da interface genérica IGenericRepository
    // e será específica para a entidade Product
    public interface IProductRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();


        //Método para gerar uma lista de produtos
        IEnumerable<SelectListItem> GetComboProducts();
    }
}
