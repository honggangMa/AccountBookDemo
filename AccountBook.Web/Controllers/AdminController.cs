using System;
using System.Linq;
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
        public IActionResult Main()
        {
            return View();
        }
        public IActionResult ExpenseList()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetExpenseList(int page=1, int limit=15, string key=null)
        {
            var data = from e in _dbContext.Expense select e;
            if (!string.IsNullOrEmpty(key))
            {
                data = data.Where(d => d.Remark.Contains(key));
            }                     
            data = data.AsNoTracking().OrderByDescending(d=>d.CreateTime).Skip(limit * (page - 1)).Take(limit);
            return Json(new { data = data.ToList(),msg="ok" ,count = data.Count(), code = 0 });
        }
        [HttpGet]
        public IActionResult EditExpense(int id = 0)
        {
            if (id == 0)
            {
                ViewData.Model = new Expense();
            }
            else {
              var model=  _dbContext.Expense.Where(e => e.Id == id).FirstOrDefault();             
                if (model!=null)
                {
                    ViewData.Model = model;
                }
            }
            return View();        
        }
        [HttpPost]
        public string EditExpense(Expense expense)
        {
            if (expense.Id != 0 && !string.IsNullOrEmpty(expense.Id.ToString()))
            {
                var model = _dbContext.Expense.Where(e => e.Id == expense.Id).FirstOrDefault();
                if (model!=null)
                {
                    model.UpdateTime = DateTime.Now;                   
                    model.MorningMoney = expense.MorningMoney;
                    model.EveningMoney = expense.EveningMoney;
                    model.AfternoonMoney = expense.AfternoonMoney;
                    model.MoreMoney = expense.MoreMoney;
                    model.Remark = expense.Remark;
                    _dbContext.Entry(model).State = EntityState.Modified;
                }                                          
            }
            else {
                expense.CreateTime = DateTime.Now;
                _dbContext.Expense.Add(expense);              
            }
            return _dbContext.SaveChanges() > 0 ? "ok" : "no";
        }
        public string DeleteExpense(int id)
        {
            Expense expense = new Expense() { Id=id};
            _dbContext.Entry(expense).State = EntityState.Deleted;
            
            return _dbContext.SaveChanges() > 0 ? "ok" : "no";
        }
    }
}