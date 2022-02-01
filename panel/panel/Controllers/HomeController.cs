using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
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
using panel.Extensions;

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
            string trycount = "1";
            var userdata = await _loginRepo.Login(StaticDetail.StaticDetails.login, userDto);
            if (string.IsNullOrEmpty(userdata.Token))
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userdata.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, userdata.Role.RoleName));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            byte[] token = Encoding.UTF8.GetBytes(userdata.Token);
            HttpContext.Session.Set("Jwt", token);
            return RedirectToAction("Index", "Home");
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

        [AllowAnonymous]
        [HttpGet("forgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgotPasswordContent")]
        public async Task<IActionResult> ForgotPasswordContent(string username)
        {
            try
            {
                UserDto userdata = await _loginRepo.GetUserDataByName(StaticDetail.StaticDetails.getByUserName, username);
                string passstring = StringProcess.GenerateString();
                User user = new()
                { Id = userdata.Id, Password = userdata.Password, ResetPassword = passstring, RoleId = userdata.RoleId, UserName = userdata.UserName };

                var updatedUser = await _loginRepo.Update(StaticDetail.StaticDetails.mainUrl + "api/user/updateResetUser", user);
                string link = StaticDetail.StaticDetails.currentUrl + "updateuserpass/" + passstring;
                MimeMessage mG = new MimeMessage();
                BodyBuilder bodyBuilderG = new BodyBuilder();
                SmtpClient smtpG = new SmtpClient();
                bodyBuilderG.HtmlBody = string.Format(@"<p>Talep eden kullanıcı: <a href='mailto: {0}'>{1}</a></p>
                                                        <br>
                                                        <p>Klulanıcının şifresini yenilemek için aşağıdaki linki paylaşmalısınız.</p>
                                                        <p>Link: <a href={2}</a></p>", username, username, link);
                MailboxAddress fromG = new MailboxAddress("Şifre Yenileme Talebi", "info@camasirhaneekipmanlari.com");
                MailboxAddress toG = new MailboxAddress(username, "ugur.yalcin@camasirhaneekipmanlari.com");
                mG.From.Add(fromG);
                mG.Body = bodyBuilderG.ToMessageBody();
                mG.Subject = "Şifre Yenileme Talebi";
                mG.To.Add(toG);
                smtpG.Connect("win5.wlsrv.com", 465, true);
                smtpG.Authenticate("info@camasirhaneekipmanlari.com", "Yamahar123+-*/");
                smtpG.Send(mG);
                smtpG.Disconnect(true);
                smtpG.Dispose();

                if (userdata == null && !updatedUser)
                {
                    return Json(0);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(0);
            }
        }

        [AllowAnonymous]
        [HttpPost("updatepassword")]
        public async Task<IActionResult> UpdatePassword(User user)
        {
            return Ok();
        }

        [AllowAnonymous]
        [Route("updateuserpass/{passstring}")]
        public async Task<IActionResult> UpdateUserPass(string passstring)
        {
            if (!string.IsNullOrEmpty(passstring))
            {
                ViewBag.passstring = passstring;
                return View();
            }
            return RedirectToRoute("/acccessdenied");
        }

        [AllowAnonymous]
        [HttpPost("updatepass")]
        public async Task<IActionResult> UpdatePass(User user)
        {
            var userData = await _loginRepo.Get(StaticDetail.StaticDetails.mainUrl + "api/user/getByResetPass/" + user.ResetPassword);
            if (userData == null)
            {
                return RedirectToAction("AccessDenied");
            }
            User updetUser = new()
            { Id = userData.Id, Password = user.Password, ResetPassword = null, RoleId = userData.RoleId, UserName = userData.UserName };
            var updatedUser = await _loginRepo.Update(StaticDetail.StaticDetails.mainUrl + "api/user/updateResetUser", updetUser);
            if (!updatedUser)
            {
                return RedirectToAction("AccessDenied");
            }
            return RedirectToRoute("Login");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
