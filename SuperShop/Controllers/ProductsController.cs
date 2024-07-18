using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;

namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository _repository;

        //Ctrl  + . em cima do repository e clicar em "Create and assign field repository"
        public ProductsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: Products
        //Retorna a View e vai buscar todos os Produtos
        //esta accion Index a unica coisa que faz vai ao _repository vai à propriedade dos Products e
        //traz todos os produtos ToListAsync para dentro da view
        public IActionResult Index()
        {
            return View(_repository.GetProducts());
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Aqui pomos o .Value no id para quando o valor for nulo a aplicaçao nao crashar
            //Sempre que o parametro estiver como opcional que é o que acontece aqui pois em cima
            //tem "int?" em que o "?" significa que o utilizador pode ou nao colcar o id temos que por .Value para nao crashar
            var product = _repository.GetProduct(id.Value);
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
        public async Task<IActionResult> Create(Product product)
        {   //Aqui vê se o produto é valido cumprindo as regras que demos no ficheiro Product.cs
            if (ModelState.IsValid)
            {   //Se o produto for válido adicionamos o produto em memoria (nao grava na base de dados fica pendente)
                _repository.AddProduct(product);
                //O produto só é gravado aqui na base de dados e de uma forma assincrona
                await _repository.SaveAllAsync();
                //No final de gravar redireciona para accion Index
                return RedirectToAction(nameof(Index));
            }
            //Se acontecer algum problema com o produta nao passando nas validaçoes mostra a mesma View mas deixa lá
            //ficar o produta para a pessoa não estar a escrever tudo de novo
            return View(product);
        }

        // GET: Products/Edit/5
        //Aqui temos o "?" que serve para nao forçar a pessoa a por um ID e deixar passar em branco sendo opcional
        //sendo assim nao é preciso ter um ID exemplo: podendo ficar assim "https://localhost:44369/Products/edit" na vez de 
        //"https://localhost:44369/Products/edit/1"
        public IActionResult Edit(int? id)
        {   //Se o Id for nulo retornaNotFound
            if (id == null)
            {
                return NotFound();
            }
            //Se o ID nao for nulo vai ver á tabela se o ID existe
            var product = _repository.GetProduct(id.Value);
            //Aqui volta de novo a verificar o ID 
            if (product == null)
            {
                return NotFound();
            }
            //se encontrar o produto retorna view e manda o produto
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Isto é chamado quando carregamos no botao submit do Edit.cshtml, recebendo o id e o produto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {   //Vemos se o id existe ou nao
            if (id != product.Id)
            {
                return NotFound();
            }

            //caso esteja tudo ok vemos se o Modelo é valido
            if (ModelState.IsValid)
            {
                try
                {   //faz o update do produto
                    _repository.UpdateProduct(product);
                    //Grava o produto
                    await _repository.SaveAllAsync();
                }
                //Caso aconteça alguma coisa mal vê o que se passou
                catch (DbUpdateConcurrencyException)
                {//Se acontecer alguma coisa mal vai verificar o ID pois como na web pode haver muitas pessoas a trabalhar em
                 //simultaneo e se por exemplo eu tiver a preencher mas alguem ja apagou o produto tendo esta validação a app nao rebenta
                    if (!_repository.ProductExists(product.Id))
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
            //Se alguma coisa correr mal retorna na mesma a View com o produto dentro
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {   //Se o Id nao existe retorna NotFound
            if (id == null)
            {
                return NotFound();
            }
            //SE o ID existe vai buscar à tabela
            var product = _repository.GetProduct(id.Value);
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
            var product = _repository.GetProduct(id);
            //Aqui remove em memória
            _repository.RemoveProduct(product);
            //Aqui vai à base de dados
            await _repository.SaveAllAsync();
            //Depois quando apagar redireciona para a view Index
            return RedirectToAction(nameof(Index));
        }
    }
}
