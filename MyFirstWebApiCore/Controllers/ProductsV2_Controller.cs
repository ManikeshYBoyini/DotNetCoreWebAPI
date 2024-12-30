using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApiCore.Classes;
using MyFirstWebApiCore.Models;

namespace MyFirstWebApiCore.Controllers
{
    [ApiVersion("2.0")]
    [Route("v{v:apiversion}/Products")]
    [ApiController]
    public class ProductsV2_Controller : ControllerBase
    {
        private readonly ShopContext _db;
        public ProductsV2_Controller(ShopContext db)
        {
            _db = db;
        }
        [HttpGet, Route("Products")]
        public IEnumerable<Product> GetProducts()
        {
            return _db.Products.Where(x=>x.Name.Contains("a")).ToList();
        }

        [HttpGet, Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParams)
        {
            IQueryable<Product> products = _db.Products;

            products = products.Skip(queryParams.Size * (queryParams.Page - 1)).Take(queryParams.Size);

            if (queryParams.MaxPrice != null && queryParams.MinPrice != null)
            {
                products = products.Where(x => x.Price >= queryParams.MinPrice && x.Price <= queryParams.MaxPrice);
            }
            if (!string.IsNullOrEmpty(queryParams.sku))
            {
                products = products.Where(x => x.Sku.ToLower().Contains(queryParams.sku.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                products = products.Where(x => x.Name.ToLower().Contains(queryParams.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParams.OrderBy))
            {
                if (typeof(Product).GetProperty(queryParams.OrderBy) != null)
                {
                    products = products.OrderByCustom(queryParams.OrderBy, queryParams.OrderOrder);
                }
            }

            return Ok(await products.ToArrayAsync());
        }

        [HttpGet, Route("Products/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var list = await _db.Products.FindAsync(id);
            if (list == null) return NotFound();
            return Ok(list);
        }

        [HttpGet, Route("LoadProducts")]
        public bool LoadProducts()
        {
            var productList = _db.GetProducts();
            foreach (var prod in productList)
            {
                _db.Products.Add(prod);
            }
            _db.Database.ExecuteSqlCommand("Set identity_insert dbo.Products ON");
            _db.SaveChanges();
            _db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT DBO.PRODUCTS OFF");
            return true;
        }

        [HttpPost, Route("AddProducts")]
        public async Task<ActionResult<Product>> AddProducts([FromBody]Product request)
        {
            _db.Products.Add(request);
            await _db.SaveChangesAsync();
            return CreatedAtAction(
                "GetProductById", new { id = request.Id }, request);
        }

        [HttpPut, Route("PutProduct/{id}")]
        public async Task<IActionResult> Products(int id, Product request)
        {
            if (id != request.Id)
                return BadRequest("Invalid input");

            _db.Entry(request).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (_db.Products.Find(id) == null)
                    return NotFound();
                throw;
            }
            return Ok("product is successfully updated");
        }

        [HttpDelete, Route("DelProduct/{id}")]
        public async Task<ActionResult<Product>> DeleteProducts(int request)
        {
            var response = await _db.Products.FindAsync(request);
            if (response == null)
                return NotFound();

            _db.Products.Remove(response);
            await _db.SaveChangesAsync();
            return response;
        }
    }
}