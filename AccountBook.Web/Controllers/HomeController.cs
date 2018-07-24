using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccountBook.Web.Models;
using AccountBook.Repository;

namespace AccountBook.Web.Controllers
{
    public class HomeController : Controller
    {
        public UserInfoRepository _UserInfoRepository;
         public HomeController(UserInfoRepository userInfoRepository)
        {
            _UserInfoRepository = userInfoRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Test2()
        {
            var data = _UserInfoRepository.GetList();
            return View(data);
        }
    }
}
