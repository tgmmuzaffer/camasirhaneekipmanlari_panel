using Microsoft.AspNetCore.Mvc;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepo _tagRepo;
        public TagController(ITagRepo tagRepo)
        {
            _tagRepo = tagRepo;
        }
        [Route(template: "addTag", Name = "Tag Ekle")]
        public async Task<IActionResult> AddTag()
        {            
            return View();
        }

        [HttpPost("createTag")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTag([FromForm] Tag tag)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _tagRepo.Create(StaticDetail.StaticDetails.createTag, tag, token);

            return RedirectToAction("GetAllTags");
        }

        [Route(template: "getAllTags", Name = "Tag Listesi")]
        public async Task<IActionResult> GetAllTags()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var tags = await _tagRepo.GetList(StaticDetail.StaticDetails.getAllTags, token);
            return View(tags);
        }

        [HttpGet("updateTag/{Id}")]
        public async Task<IActionResult> UpdateTag(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var tag = await _tagRepo.Get(StaticDetail.StaticDetails.getTag+ Id, token);
            return View(tag);
        }
        [HttpPost("updateTagContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTagContent([FromForm] Tag tag)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _tagRepo.Update(StaticDetail.StaticDetails.updateTag, tag, token);

            return RedirectToAction("GetAllTags");
        }

        [Route("deleteTag/{id}")]
        public async Task<IActionResult> DeleteTag(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _tagRepo.Delete(StaticDetail.StaticDetails.deleteTag + Id, token);
            if (result)
            {
                TempData["success"] = "Tag silindi.  ";
            }
            else
            {
                TempData["fail"] = "Tag silinirken bir hata oluştu";
            }

            return RedirectToAction("GetAllTags");
        }
    }
}
