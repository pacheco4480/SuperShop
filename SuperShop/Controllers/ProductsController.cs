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
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: Products
        //Retorna a View e vai buscar todos os Produtos
        //esta accion Index a unica coisa que faz vai ao _context vai à propriedade dos Products e
        //traz todos os produtos ToListAsync para dentro da view
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(product);
                //O produto só é gravado aqui na base de dados e de uma forma assincrona
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int? id)
        {   //Se o Id for nulo retornaNotFound
            if (id == null)
            {
                return NotFound();
            }
            //Se o ID nao for nulo vai ver á tabela se o ID existe
            var product = await _context.Products.FindAsync(id);
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
                    _context.Update(product);
                    //Grava o produto
                    await _context.SaveChangesAsync();
                }
                //Caso aconteça alguma coisa mal vê o que se passou
                catch (DbUpdateConcurrencyException)
                {//Se acontecer alguma coisa mal vai verificar o ID pois como na web pode haver muitas pessoas a trabalhar em
                 //simultaneo e se por exemplo eu tiver a preencher mas alguem ja apagou o produto tendo esta validação a app nao rebenta
                    if (!ProductExists(product.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {   //Se o Id nao existe retorna NotFound
            if (id == null)
            {
                return NotFound();
            }
            //SE o ID existe vai buscar à tabela
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var product = await _context.Products.FindAsync(id);
            //Aqui remove em memória
            _context.Products.Remove(product);
            //Aqui vai à base de dados
            await _context.SaveChangesAsync();
            //Depois quando apagar redireciona para a view Index
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
