using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApiCore.Models;

namespace MyFirstWebApiCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopContext _db;
        public CategoryController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet,Route("LoadCategories")]
        public bool LoadCategory()
        {
            var categoryList = _db.SetCategory();

            _db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT CATEGORIES ON");
            foreach (var cat in categoryList)
            {
                _db.Categories.Add(cat);
            }
            _db.SaveChanges();
            _db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT CATEGORIES OFF");
            return true;
        }

    }
}