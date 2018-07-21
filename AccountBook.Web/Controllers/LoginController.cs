using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountBook.Common;
using AccountBook.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountBook.Web.Controllers
{
    public class LoginController : BaseController
    {
       
        public LoginController(AppliactionDbContext dbContext):base(dbContext)
        {
          
        }
        [HttpGet]
        public IActionResult Index(string ReturnUrl)
        {
         
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(string userName,string pwd,string vcode)
        {
            try
            {
                var session = HttpContext.Session.GetString("vccode") == null ? "" : HttpContext.Session.GetString("vccode").ToString();
                if (string.IsNullOrEmpty(vcode) || session != vcode.ToString())
                {
                    return Content("no,提示：验证码错误！");
                }
                var model = _dbContext.Users.Where(u => u.UserName == userName && u.Password == pwd).FirstOrDefault();
                if (model != null)
                {
                    var claims = new[] {
                    new Claim("username",model.UserName),
                    new Claim("userid",model.Id.ToString())
                };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties() { IsPersistent = true, ExpiresUtc = DateTime.Now.AddMinutes(60) }).Wait();
                    CommonOperatorLogData("用户登录",1);
                    return Content("ok,恭喜：登录成功");
                }
                else
                {
                    CommonOperatorLogData("用户登录", 0);
                    return Content("no,提示：用户名或密码错误");
                }
            }
            catch (Exception ex)
            {
                throw ex;
               // CommonOperatorLogData(ex.ToString());
               
            }
            
        }
        public FileResult VCode()
        {
            var vcode = VerifyCode.CreateRandomCode(5);
            HttpContext.Session.SetString("vccode",vcode);
            var img = VerifyCode.DrawImage(vcode, 20, Color.White);
            return File(img, "image/gif");
        }
        public  IActionResult LogOut()
        {
             HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Login");
        }
    }
}