using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class BaseController : Controller
    {
        public string  GetToken()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            else if(value == null || value.Length ==0)
            {
                RedirectToAction("Login", "Home");
            }

            return token;
        }

        public string GetRole()
        {
            HttpContext.Session.TryGetValue("UserRole", out byte[] rolevalue);
            string role = string.Empty;
            if (rolevalue != null && rolevalue.Length > 0)
            {
                role = Encoding.Default.GetString(rolevalue);
            }
            else if(rolevalue != null && rolevalue.Length > 0)
            {
                RedirectToAction("AccessDenied", "Home");
            }
            return role;
        }
    }
}
