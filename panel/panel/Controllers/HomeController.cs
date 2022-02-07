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
using panel.StaticDetail.Enums;

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

        //anasayfa
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

        //login sayfasını açar
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        //login işlemini yapar
        [AllowAnonymous]
        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser(UserDto userDto)
        {
            var userdata = await _loginRepo.Login(StaticDetail.StaticDetails.login, userDto);
            if (string.IsNullOrEmpty(userdata.Token))
            {
                return RedirectToAction("AccessDenied");
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userdata.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, userdata.Role.RoleName));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            byte[] token = Encoding.UTF8.GetBytes(userdata.Token);
            byte[] role = Encoding.UTF8.GetBytes(userdata.RoleId.ToString());
            HttpContext.Session.Set("Jwt", token);
            HttpContext.Session.Set("UserRole", role);

            return RedirectToAction("Index", "Home");
        }

        //Hesap oluştur sayfasını açar
        [AllowAnonymous]
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        //ilk hesap oluşturma 
        [AllowAnonymous]
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            if (userDto == null || userDto.RoleId <= 0)
            {
                userDto.RoleId = (int)Roles.Role.Editor;
            }

            UserDto userdata = await _loginRepo.Login(StaticDetail.StaticDetails.register, userDto);
            if (string.IsNullOrEmpty(userdata.Token))
            {
                return RedirectToAction("Login");
            }
            byte[] token = Encoding.UTF8.GetBytes(userdata.Token);
            byte[] userole = Encoding.UTF8.GetBytes(userdata.RoleId.ToString());
            HttpContext.Session.Set("Jwt", token);
            HttpContext.Session.Set("UserRole", userole);

            return RedirectToAction("Login");
        }

        //Çıkış yap 
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            byte[] token = { };
            _ = HttpContext.SignOutAsync();
            HttpContext.Session.Set("Jwt", token);
            return RedirectToAction("Index");
        }

        //yetkisiz işlem sayfası
        [AllowAnonymous]
        [HttpGet("acccessdenied")]
        public IActionResult AccessDenied()
        {
            int deniedCount = 1;
            HttpContext.Session.TryGetValue("DENIED", out byte[] value);
            if (value == null)
            {
                byte[] deniedbytes = BitConverter.GetBytes(deniedCount);
                HttpContext.Session.Set("DENIED", deniedbytes);
            }
            else
            {
                int newdeniedCount = BitConverter.ToInt32(value) + 1;
                byte[] newdeniedbytes = BitConverter.GetBytes(newdeniedCount);
                HttpContext.Session.Set("DENIED", newdeniedbytes);
            }
            return View();
        }

        //Şifremi unuttum sayfasını açar
        [AllowAnonymous]
        [HttpGet("forgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //Şifremi yenileme talebi gönderir
        [HttpPost("forgotPasswordContent")]
        public async Task<IActionResult> ForgotPasswordContent(string username)
        {
            try
            {
                UserDto userdata = await _loginRepo.GetUserDataByName(StaticDetail.StaticDetails.getByUserName, username);
                string passstring = StringProcess.GenerateString();
                User user = new()
                {
                    Id = userdata.Id,
                    Password = null,
                    ResetPassword = passstring,
                    RoleId = userdata.RoleId,
                    UserName = userdata.UserName,
                    Salt = null
                };

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

        //oturum sahibinin bilgilerini getirir
        [Authorize(Roles = "Admin, Editor")]
        [HttpGet( "updatemydata/{usermail}")]
        public async Task<IActionResult> UpdateMyData(string usermail)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }
            
            var mydata = await _loginRepo.Get(StaticDetail.StaticDetails.getMyData + usermail, token);
            UserDto userDto = new() { Id = mydata.Id, UserName = mydata.UserName };
            return View(userDto);
        }

        //oturum sahibi rol hariç diğer bilgilerini günceller
        [Authorize(Roles = "Admin, Editor")]
        [HttpPost("updatemydatacontent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMyDataContent(User user)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            bool isUpdated = await _loginRepo.Update(StaticDetail.StaticDetails.updateMyDataContent, user, token);
            if (isUpdated)
            {
                TempData["success"] = "Kullanıcı bilgileri güncellendi ";
            }
            else
            {
                TempData["fail"] = "Kullanıcı bilgileri güncellenemedi. ";
            }
            return RedirectToAction("Index");
        }

        //Kullanıcıya gelen şifremi unuttum maili ile şifre güncelleme sayfasını açar
        [AllowAnonymous]
        [Route("updateuserpass/{passstring}")]
        public async Task<IActionResult> UpdateUserPass(string passstring)
        {
            if (!string.IsNullOrEmpty(passstring))
            {
                ViewBag.passstring = passstring;
                return View();
            }
            return RedirectToAction("/acccessdenied");
        }

        //Şifremi unutum sayfasından gelen datalar ile değiştirme işlemini yapar
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
            return RedirectToAction("Login");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
