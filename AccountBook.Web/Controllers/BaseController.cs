using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Common;
using AccountBook.Model;
using Microsoft.AspNetCore.Mvc;

namespace AccountBook.Web.Controllers
{
    public class BaseController : Controller
    {
        public readonly AppliactionDbContext _dbContext;
        public BaseController(AppliactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CommonOperatorLogData(string str,int? state=null)
        {
            try
            {
                Log log = new Log();
                if (state.HasValue)
                {
                    if (state > 0)
                        log.State = true;
                    else
                        log.State = false;
                }
                log.CreateTime = DateTime.Now;
                log.IP = Net.Ip;
                log.Content = str;
                log.Address = Net.GetIPCitys(Net.Ip);
                log.User = new UserInfo() { UserName= UseCookieGetCurrentUserName() };//int.Parse(HttpContext.User.FindFirst("userid")?.Value??"0");
                _dbContext.Log.Add(log);
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }          
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

    }
}