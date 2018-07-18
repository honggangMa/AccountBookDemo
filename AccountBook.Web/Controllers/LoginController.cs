using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Model;
using Microsoft.AspNetCore.Mvc;

namespace AccountBook.Web.Controllers
{
    public class LoginController : Controller
    {
        public readonly AppliactionDbContext _dbContext;
        public LoginController(AppliactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
           var data= _dbContext.Users.ToList();
            return Json(data);
        }
    }
}