using LoginFormAspCore8.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LoginFormAspCore8.Controllers
{
    public class HomeController : Controller
    {
        private readonly CodeSecondDbContext context;

        public HomeController(CodeSecondDbContext context)
        {
            this.context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("USerSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserTbl user)
        {
            var myUser = context.UserTbls.Where(x =>x.Email==user.Email && x.Password==user.Password).FirstOrDefault( );
            if(myUser != null)
            {
                HttpContext.Session.SetString("UserSession",myUser.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed";
            }
            return View();
        }
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("USerSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("USerSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("USerSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserTbl user)
        {
            if (ModelState.IsValid)
            {
                await context.UserTbls.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Success"] = "Register successfully";
                return RedirectToAction("Login");

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
