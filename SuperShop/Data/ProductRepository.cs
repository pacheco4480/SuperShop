using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SuperShop.Data
{
    // A classe ProductRepository herda da classe genérica GenericRepository
    // e implementa a interface específica IProductRepository
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;

        // Construtor da classe ProductRepository que aceita um DataContext como parâmetro
        // e passa este contexto para a classe base (GenericRepository)
        public ProductRepository(DataContext context) : base(context)
        {
            // O construtor chama o construtor da classe base com o parâmetro "context"
            // Isto inicializa o contexto na classe base GenericRepository
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {   //Isto é como se tivessemos a fazer um Inner Join em SQl
            //Aqui relaciona as tabelas Produtos e Users
            return _context.Products.Include(p => p.User);
        }

        // Método para gerar uma lista de produtos como SelectListItem, que é útil para popular dropdowns em views.
        // Cada SelectListItem possui um texto (nome do produto) e um valor (ID do produto).
        public IEnumerable<SelectListItem> GetComboProducts()
        {
            // Cria uma lista de SelectListItem a partir da lista de produtos no contexto.
            var list = _context.Products.Select(p => new SelectListItem
            {
                // Define o nome do produto como texto do item.
                Text = p.Name,
                // Define o ID do produto como valor do item, convertido para string.
                Value = p.Id.ToString()
            }).ToList();

            // Insere um item no início da lista para representar a opção de seleção padrão.
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a product...)", // Texto exibido na dropdown
                Value = "0" // Valor padrão para indicar que nenhum produto foi selecionado
            });

            return list;
        }
    }
}
