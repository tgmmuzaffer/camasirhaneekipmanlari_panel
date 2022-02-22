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
    public class TagController : BaseController
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
            string token = GetToken();
            var res = await _tagRepo.Create(StaticDetail.StaticDetails.createTag, tag, token);

            return RedirectToAction("GetAllTags");
        }

        [Route(template: "getAllTags", Name = "Tag Listesi")]
        public async Task<IActionResult> GetAllTags()
        {
            string token = GetToken();
            var tags = await _tagRepo.GetList(StaticDetail.StaticDetails.getAllTags, token);
            return View(tags);
        }

        [HttpGet("updateTag/{Id}")]
        public async Task<IActionResult> UpdateTag(int Id)
        {
            string token = GetToken();
            var tag = await _tagRepo.Get(StaticDetail.StaticDetails.getTag+ Id, token);
            return View(tag);
        }
        [HttpPost("updateTagContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTagContent([FromForm] Tag tag)
        {
            string token = GetToken();
            var res = await _tagRepo.Update(StaticDetail.StaticDetails.updateTag, tag, token);

            return RedirectToAction("GetAllTags");
        }

        [Route("deleteTag/{id}")]
        public async Task<IActionResult> DeleteTag(int Id)
        {
            string token = GetToken();
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
