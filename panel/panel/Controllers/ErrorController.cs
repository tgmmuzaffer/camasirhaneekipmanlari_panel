using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class ErrorController : Controller
    {
        [Route("notfound")]
        public IActionResult Notfound()
        {
            return View();
        }

        [Route("ise")]
        public IActionResult Ise()
        {
            return View();
        }
    }
}
