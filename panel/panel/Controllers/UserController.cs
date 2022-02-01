using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        public UserController(IUserRepo userRepo, IRoleRepo roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        [HttpGet(template:"panelusers", Name ="Panel Kullanıcıları")]
        public async Task<IActionResult> PanelUsers()
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            var resultUser = await _userRepo.GetList(StaticDetail.StaticDetails.getAllUser, token);
            return View(resultUser);
        }

        [Route(template:"addpaneluser", Name ="Panel Kullanıcısı Ekle")]
        public async Task<IActionResult> AddPanelUser()
        {

            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            var resultRole = await _roleRepo.GetRoles(StaticDetail.StaticDetails.getRoles, token);
            List<SelectListItem> roleList = new();
            foreach (var item in resultRole)
            {
                roleList.Add(new SelectListItem() { Text = item.RoleName, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost("addpaneluser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPanelUser(UserDto userDto)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            userDto.RoleId = userDto.Role.Id;
            userDto.Role = null;
            var result = await _userRepo.Create(StaticDetail.StaticDetails.registerUser, userDto, token);
            return Redirect("PanelUsers");
        }

        [Route("deletepaneluser/{id}")]
        public async Task<IActionResult> DeletePanelUser(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _userRepo.Delete((StaticDetail.StaticDetails.deleteUser)+Id, token);
            if (result)
            {
                TempData["success"] = "Kullanıcı silindi.  ";
            }
            else
            {
                TempData["fail"] = "Kullanıcı silinirken bir hata oluştu";
            }

            return RedirectToAction("PanelUsers");
        }

        [HttpGet("updatepaneluser/{id}")]
        public async Task<IActionResult> UpdatePanelUser(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var model = await _userRepo.Get((StaticDetail.StaticDetails.getUser) + Id, token);
            var resultRole = await _roleRepo.GetRoles(StaticDetail.StaticDetails.getRoles, token);
            List<SelectListItem> roleList = new();
            foreach (var item in resultRole)
            {
                roleList.Add(new SelectListItem() { Text = item.RoleName, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.RoleList = roleList;

            return View(model);
        }

        [HttpPost("updateuser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            userDto.RoleId = userDto.Role.Id;
            bool result = await _userRepo.Update(StaticDetail.StaticDetails.updateUser, userDto, token);
            if (result)
            {
                TempData["success"] = "Kullanıcı güncellendi.  ";
            }
            else
            {
                TempData["fail"] = "Kullanıcı güncellenirken bir hata oluştu";
            }

            return RedirectToAction("PanelUsers");
        }

       
    }
}
