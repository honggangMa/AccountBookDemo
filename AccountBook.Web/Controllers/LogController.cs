using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Model;
using AccountBook.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountBook.Web.Controllers
{
    [Authorize]
    public class LogController : BaseController
    {
        public LogController(AppliactionDbContext dbContext) : base(dbContext)
        { }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetLogList(int page = 1, int limit = 15, string key = null)
        {
            var data = from e in _dbContext.Log select e;
            if (!string.IsNullOrEmpty(key))
            {
                data = data.Where(d => d.Content.Contains(key));
            }
            var rows = data.Include(u=>u.User).AsNoTracking().OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit).Select(n=>new LogViewModel() {
                                UserName=n.User.UserName,
                                Id=n.Id,
                                CreateTime=n.CreateTime,
                                IP=n.IP,
                                State=n.State,
                                Content=n.Content,
                                Address=n.Address
            });
            return Json(new { data = rows.ToList(), msg = "ok", count = data.Count(), code = 0 });
        }
       
        public string DeleteLog(int id)
        {
            Log log = new Log() { Id = id };
            _dbContext.Entry(log).State = EntityState.Deleted;
            CommonOperatorLogData("删除一条日志信息");
            return _dbContext.SaveChanges() > 0 ? "ok" : "no";
        }
        public IActionResult DetailLog(int id)
        {
            ViewData.Model = _dbContext.Log.Where(l => l.Id == id).FirstOrDefault();
            CommonOperatorLogData("查看一条日志信息");
            return View();
        }
    }
}