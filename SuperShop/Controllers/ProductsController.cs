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
        //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
        //private readonly IImageHelper _imageHelper;
        //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        //Ctrl  + . em cima do productRepository e clicar em "Create and assign field productRepository"
        //Ctrl  + . em cima do userHelper e clicar em "Create and assign field userHelper"
        //Ctrl  + . em cima do imageHelper e clicar em "Create and assign field imageHelper"
        //Ctrl  + . em cima do converterHelper e clicar em "Create and assign field converterHelper"

        public ProductsController(IProductRepository productRepository,
                                  IUserHelper userHelper,
                                  //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                                  //IImageHelper imageHelper,
                                  //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                                  IBlobHelper blobHelper,
                                  IConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
            //_imageHelper = imageHelper;
            //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
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
            {   //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                //Carregar as Imagens
                //var path = string.Empty;
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                Guid imageId = Guid.Empty;

                //Caso tenha uma imagem
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {   //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                    //Recebe o ficheiro, guarda na pata products e vai UploadImageAsync que está IImageHelper 
                    //path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                    //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");

                }
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                //Converter o ProductViewModel em Product
                //Mandamos o model, path, e como o produto é novo mandamos true
                //var product = _converterHelper.ToProduct(model, path, true);
                //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                var product = _converterHelper.ToProduct(model, imageId, true);

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


            var model = _converterHelper.ToProductViewModel(product);

            //se encontrar o produto retorna view e manda o model
            return View(model);
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
                {   //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                    //var path = model.ImageUrl;
                    //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                    Guid imageId = model.ImageId;

                    //Caso tenha uma imagem
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {   //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                        //Recebe o ficheiro, guarda na pata products e vai UploadImageAsync que está IImageHelper 
                        //path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                        //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    }
                    //SUBSTITUIÇAO IMAGEURL por IMAGEID - OLD
                    //Mandamos o model, path, e como o produto é nao é novo mandamos false
                    //var product = _converterHelper.ToProduct(model, path, false);
                    //SUBSTITUIÇAO IMAGEURL por IMAGEID - NEW
                    var product = _converterHelper.ToProduct(model, imageId, false);

                    //TODO: Modifiar para o user que estiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("david@gmail.com");
                    //faz o update do produto
                    await _productRepository.UpdateAsync(product);
                }
                //Caso aconteça alguma coisa mal vê o que se passou
                catch (DbUpdateConcurrencyException)
                {//Se acontecer alguma coisa mal vai verificar o ID pois como na web pode haver muitas pessoas a trabalhar em
                 //simultaneo e se por exemplo eu tiver a preencher mas alguem ja apagou o produto tendo esta validação a app nao rebenta
                    if (!await _productRepository.ExistAsync(model.Id))
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
