using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        //Ctrl  + . em cima do productRepository e clicar em "Create and assign field productRepository"
        //Ctrl  + . em cima do userHelper e clicar em "Create and assign field userHelper"

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            // Retorna a View e vai buscar todos os produtos usando o método GetAll do repositório e ordena
            //os produtos por nome
            return View(_productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Aqui pomos o .Value no id para quando o valor for nulo a aplicaçao nao crashar
            //Sempre que o parametro estiver como opcional que é o que acontece aqui pois em cima
            //tem "int?" em que o "?" significa que o utilizador pode ou nao colcar o id temos que por .Value para nao crashar
            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        //A unica coisa que esta View faz é abrir a View do create
        //Precisamos sempre das duas GET e POST uma mostra e outra recebe os valores
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Esta View é a que é chamada quando clicamos no botao "Create" sendo a responsavel por receber o
        //modelo e manda-la para a base de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {   //Aqui vê se o produto é valido cumprindo as regras que demos no ficheiro Product.cs
            if (ModelState.IsValid)
            {
                //Carregar as Imagens
                var path = string.Empty;
                //Caso tenha uma imagem
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {   //Aqui gera uma chave aleatoria que no vai dar um objeto do tipo Guid e vamos converter em string
                    //Isto faz com que seja gerado um nome 
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot\\images\\products",
                           file);

                    //Gravar
                    using(var stream = new FileStream(path, FileMode.Create))
                    {   //Na pratica sera aqui que vai guardar
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    //Este será o caminho que vai ser guardado na base de dados
                    path = $"~/images/products/{file}";
                }

                //Converter o ProductViewModel em Product
                var product = this.ToProduct(model, path);

                //TODO: Modifiar para o user que estiver logado
                product.User = await _userHelper.GetUserByEmailAsync("david@gmail.com");
                //Se o produto for válido adicionamos o produto em memoria (nao grava na base de dados fica pendente)
                await _productRepository.CreateAsync(product);
                //No final de gravar redireciona para accion Index
                return RedirectToAction(nameof(Index));
            }
            //Se acontecer algum problema com o produta nao passando nas validaçoes mostra a mesma View mas deixa lá
            //ficar o produta para a pessoa não estar a escrever tudo de novo
            return View(model);
        }

        //Conversão do ProductViewModel em Product
        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
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

        // GET: Products/Edit/5
        //Aqui temos o "?" que serve para nao forçar a pessoa a por um ID e deixar passar em branco sendo opcional
        //sendo assim nao é preciso ter um ID exemplo: podendo ficar assim "https://localhost:44369/Products/edit" na vez de 
        //"https://localhost:44369/Products/edit/1"
        public async Task<IActionResult> Edit(int? id)
        {   //Se o Id for nulo retornaNotFound
            if (id == null)
            {
                return NotFound();
            }
            //Se o ID nao for nulo vai ver á tabela se o ID existe
            var product = await _productRepository.GetByIdAsync(id.Value);
            //Aqui volta de novo a verificar o ID 
            if (product == null)
            {
                return NotFound();
            }

            var model = this.ToProductViewModel(product);

            //se encontrar o produto retorna view e manda o model
            return View(model);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel { 
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Isto é chamado quando carregamos no botao submit do Edit.cshtml, recebendo o id e o produto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {   
            //caso esteja tudo ok vemos se o Modelo é valido
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    //Caso tenha uma imagem
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        //Aqui gera uma chave aleatoria que no vai dar um objeto do tipo Guid e vamos converter em string
                        //Isto faz com que seja gerado um nome 
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               "wwwroot\\images\\products",
                               file);

                        //Gravar
                        using (var stream = new FileStream(path, FileMode.Create))
                        {   //Na pratica sera aqui que vai guardar
                            await model.ImageFile.CopyToAsync(stream);
                        }
                        //Este será o caminho que vai ser guardado na base de dados
                        path = $"~/images/products/{file}";
                    }

                    //Converter o ProductViewModel em Product
                    var product = this.ToProduct(model, path);

                    //TODO: Modifiar para o user que estiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("david@gmail.com");
                    //faz o update do produto
                    await _productRepository.UpdateAsync(product);
                }
                //Caso aconteça alguma coisa mal vê o que se passou
                catch (DbUpdateConcurrencyException)
                {//Se acontecer alguma coisa mal vai verificar o ID pois como na web pode haver muitas pessoas a trabalhar em
                 //simultaneo e se por exemplo eu tiver a preencher mas alguem ja apagou o produto tendo esta validação a app nao rebenta
                    if (! await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //redireciona para o index
                return RedirectToAction(nameof(Index));
            }
            //Se alguma coisa correr mal retorna na mesma a View com o model dentro
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {   //Se o Id nao existe retorna NotFound
            if (id == null)
            {
                return NotFound();
            }
            //SE o ID existe vai buscar à tabela
            var product = await _productRepository.GetByIdAsync(id.Value);
            //Se nao econtra o Produto NotFOund
            if (product == null)
            {
                return NotFound();
            }
            //Se não retorna a view com o produto dentro
            return View(product);
        }

        // POST: Products/Delete/5
        //Aqui quando houver uma action chamada "Delete" ele vai fazer o "DeleteConfirmed"
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            //Aqui remove em memória
            await _productRepository.DeleteAsync(product);
            //Depois quando apagar redireciona para a view Index
            return RedirectToAction(nameof(Index));
        }
    }
}
