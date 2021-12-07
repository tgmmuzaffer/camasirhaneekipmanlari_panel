using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Repository.IRepository;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo; 
        public UserController(IUserRepo userRepo)
        {
            _userRepo=userRepo;
        }

        [HttpGet("panelusers")]
        public async Task<IActionResult> PanelUsers()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            var resuylt = await _userRepo.GetList(StaticDetail.StaticDetails.getAllUser, token);
            List<SelectListItem> roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem() { Text = "Admin", Value = "Admin" });
            roleList.Add(new SelectListItem() { Text = "Editor", Value = "Editor" });
            ViewBag.RoleList = roleList;
            return View();
        }
        [Route("addpaneluser")]
        public IActionResult AddPanelUser()
        {
            return View();
        }
        [HttpPost("addpaneluser")]
        public IActionResult AddPanelUser(User user)
        {
            return View();
        }

    }
}
