using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginRepo _loginRepo;

        public HomeController(ILogger<HomeController> logger, ILoginRepo loginRepo)
        {
            _logger = logger;
            _loginRepo = loginRepo;
        }

        public IActionResult Index()
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value); 
            string token = string.Empty;

            if (value != null && value.Length > 0)
            {
                return View();
            }
                return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [AllowAnonymous]
        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser(UserDto userDto)
        {
            var userdata = await _loginRepo.Login(StaticDetail.StaticDetails.login, userDto);
            if (string.IsNullOrEmpty(userdata.Token))
            {
                //alert
                return View();
            }
            
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userdata.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, userdata.Role.RoleName));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            byte[] token = Encoding.UTF8.GetBytes(userdata.Token);
            HttpContext.Session.Set("Jwt", token);
            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            UserDto userdata = await _loginRepo.Login(StaticDetail.StaticDetails.login, userDto);
            if (string.IsNullOrEmpty(userdata.Token))
            {
                return View();
            }
            byte[] token = Encoding.UTF8.GetBytes(userdata.Token);
            byte[] userole = Encoding.UTF8.GetBytes(userdata.Role.RoleName);
            HttpContext.Session.Set("Jwt", token);
            HttpContext.Session.Set("UserRole", userole);

            return RedirectToAction("Index");
        }
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            byte[] token = { };
            _ = HttpContext.SignOutAsync();
            HttpContext.Session.Set("Jwt", token);
            return RedirectToAction("Index");
        }
        [HttpGet("acccessdenied")]
        public IActionResult AccessDenied()
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
