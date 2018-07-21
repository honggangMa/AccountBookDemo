using System;
using System.Linq;
using AccountBook.Common;
using AccountBook.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountBook.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
     
        public AdminController(AppliactionDbContext dbContext):base( dbContext)
        {           
        }
        public IActionResult Index()
        {
            ViewData["CurrnetUserName"] = UseCookieGetCurrentUserName();
            return View();
        }
        public IActionResult Main()
        {

            ViewData["CurrnetUserName"] = UseCookieGetCurrentUserName();
            return View();
        }

        public string UseCookieGetCurrentUserName()
        {
            //如果HttpContext.User.Identity.IsAuthenticated为true，
            //或者HttpContext.User.Claims.Count()大于0表示用户已经登录
            string userName = string.Empty;
            if (HttpContext.User.Identity.IsAuthenticated || HttpContext.User.Claims.Count() > 0)
            {
                userName = HttpContext.User.Claims.First().Value;
            }
            return userName;
        }
        public IActionResult ExpenseList()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetExpenseList(int page = 1, int limit = 15, string key = null)
        {
            var data = from e in _dbContext.Expense select e;
            if (!string.IsNullOrEmpty(key))
            {
                data = data.Where(d => d.Remark.Contains(key));
            }
            data = data.AsNoTracking().OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit);
            return Json(new { data = data.ToList(), msg = "ok", count = data.Count(), code = 0 });
        }
        [HttpGet]
        public IActionResult EditExpense(int id = 0)
        {
            if (id == 0)
            {
                ViewData.Model = new Expense();
            }
            else
            {
                var model = _dbContext.Expense.Where(e => e.Id == id).FirstOrDefault();
                if (model != null)
                {
                    ViewData.Model = model;
                }
            }
            return View();
        }
        [HttpPost]
        public string EditExpense(Expense expense)
        {
            try
            {
                if (expense.Id != 0 && !string.IsNullOrEmpty(expense.Id.ToString()))
                {
                    var model = _dbContext.Expense.Where(e => e.Id == expense.Id).FirstOrDefault();
                    if (model != null)
                    {
                        model.User.Id = int.Parse(HttpContext.User.FindFirst("userid").Value);
                        model.UpdateTime = DateTime.Now;
                        model.MorningMoney = expense.MorningMoney;
                        model.EveningMoney = expense.EveningMoney;
                        model.AfternoonMoney = expense.AfternoonMoney;
                        model.MoreMoney = expense.MoreMoney;
                        model.Remark = expense.Remark;
                        _dbContext.Entry(model).State = EntityState.Modified;
                        CommonOperatorLogData("编辑一条消费数据");
                    }
                }
                else
                {
                    expense.CreateTime = DateTime.Now;
                    expense.User.Id = int.Parse(HttpContext.User.FindFirst("userid").Value);
                    _dbContext.Expense.Add(expense);
                    CommonOperatorLogData("新增一条消费数据");
                }
                return _dbContext.SaveChanges() > 0 ? "ok" : "no";
            }
            catch (Exception ex)
            {
                CommonOperatorLogData(ex.ToString());
                throw ex;
            }

        }
        public string DeleteExpense(int id)
        {
            Expense expense = new Expense() { Id = id };
            _dbContext.Entry(expense).State = EntityState.Deleted;
            CommonOperatorLogData("删除一条消费数据");
            return _dbContext.SaveChanges() > 0 ? "ok" : "no";
        }
    }
}