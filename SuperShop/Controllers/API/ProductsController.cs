﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;

namespace SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //Ctrl  + . em cima do JwtBearerDefaults install package Microsoft.AspNetCore.Authentication.JwtBearer
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {   
        private readonly IProductRepository _productRepository;

        //Construtor
        //Ctrl  + . em cima do productRepository e clicar em "Create and assign field productRepository"
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Action que vai dar os produtos todos
        [HttpGet]
        public IActionResult GetProducts()
        {
            //Vai buscar os produtos todos atraves do Repositorio _productRepository e o "Ok" mete tudo dentro do json
            return Ok(_productRepository.GetAllWithUsers());
        }
    }
}
