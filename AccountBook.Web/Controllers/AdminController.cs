using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountBook.Web.Controllers
{
    public class AdminController : Controller
    {
        public readonly AppliactionDbContext _dbContext;
        public AdminController(AppliactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ExpenseList()
        {
            return View();
        }
        public IActionResult GetExpenseList(int page, int limit, string key)
        {
            var data = from e in _dbContext.Expense select e;
            if (!string.IsNullOrEmpty(key))
            {
                data = data.Where(d => d.Remark.Contains(key));
            }
            //.Skip(param.PageSize * (param.PageIndex - 1))
             //   .Take(param.PageSize).AsQueryable();
            data = data.AsNoTracking().OrderByDescending(d=>d.CreateTime).Skip(limit * (page - 1)).Take(limit);
            return Json(new { data = data.ToList(), count = data.Count(), code = 0 });
        }
    }
}