using System;
using System.Linq;
using AccountBook.Model;
using AccountBook.Model.ViewModel;
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
          var rows   = data.Include(d=>d.User).AsNoTracking().OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit).Select(n=>new ExpenseViewModel() {
                                                    Id =n.Id,
                                                    CreateTime =n.CreateTime,
                                                    MoreMoney=n.MoreMoney,
                                                    AfternoonMoney=n.AfternoonMoney,
                                                    MorningMoney=n.MorningMoney,
                                                    EveningMoney=n.EveningMoney,
                                                    UserName=n.User.UserName,
                                                    DaySumMoney=n.MoreMoney + n.EveningMoney+n.AfternoonMoney+n.MorningMoney,
                                                    Remark=n.Remark
                                                        });
            return Json(new { data = rows.ToList(), msg = "ok", count = data.Count(), code = 0 });
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
                        model.User= new UserInfo { UserName=UseCookieGetCurrentUserName()};
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
                    expense.User = new UserInfo { UserName = UseCookieGetCurrentUserName() };
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