using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountBook.Web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(AppliactionDbContext dbContext) :base(dbContext)
            {}
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetUserList(int page = 1, int limit = 15, string key = null)
        {
            var data = from e in _dbContext.Users select e;
            if (!string.IsNullOrEmpty(key))
            {
                data = data.Where(d => d.UserName.Contains(key));
            }
            data = data.AsNoTracking().OrderByDescending(d => d.CreateTime).Skip(limit * (page - 1)).Take(limit);
            return Json(new { data = data.ToList(), msg = "ok", count = data.Count(), code = 0 });
        }
        [HttpGet]
        public IActionResult EditUser(int id = 0)
        {
            if (id == 0)
            {
                ViewData.Model = new UserInfo();
            }
            else
            {
                var model = _dbContext.Users.Where(e => e.Id == id).FirstOrDefault();
                if (model != null)
                {
                    ViewData.Model = model;
                }
            }
            return View();
        }
        [HttpPost]
        public string EditUser(UserInfo user)
        {
            try
            {
                if (user.Id != 0 && !string.IsNullOrEmpty(user.Id.ToString()))
                {
                    var model = _dbContext.Users.Where(e => e.Id == user.Id).FirstOrDefault();
                    if (model != null)
                    {
                        model.UpdateTime = DateTime.Now;
                        model.UserName = user.UserName;
                        model.RoleName = user.RoleName;
                        _dbContext.Entry(model).State = EntityState.Modified;
                        CommonOperatorLogData("编辑一条用户信息");
                    }
                }
                else
                {
                    user.CreateTime = DateTime.Now;                  
                    _dbContext.Users.Add(user);
                    CommonOperatorLogData("新增一条用户信息");
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
            UserInfo user = new UserInfo() { Id = id };
            _dbContext.Entry(user).State = EntityState.Deleted;
            CommonOperatorLogData("删除一条用户信息");
            return _dbContext.SaveChanges() > 0 ? "ok" : "no";
        }
    }
}