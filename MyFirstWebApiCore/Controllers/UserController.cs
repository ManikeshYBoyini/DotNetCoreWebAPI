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
    public class UserController : ControllerBase
    {
        private readonly ShopContext _db;
        public UserController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet,Route("LoadUsers")]
        public bool LoadUsers()
        {
            var userList = _db.GetUsers();
            foreach (var user in userList)
            {
                _db.users.Add(user);
            }
            _db.Database.ExecuteSqlCommand("Set identity_insert dbo.Products ON");
            _db.SaveChanges();
            _db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT DBO.PRODUCTS OFF");
            return true;
        }
    }
}