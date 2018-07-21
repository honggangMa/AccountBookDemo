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
                log.User.Id = int.Parse(HttpContext.User.FindFirst("userid").Value);
                _dbContext.Log.Add(log);
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
      
    }
}