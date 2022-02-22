using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISubCategoryRepo _subCategoryRepo;

        public CategoryController(ICategoryRepo categoryRepo, ISubCategoryRepo subCategoryRepo)
        {
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;
        }

        [Route(template:"addCategory", Name ="Kategori Ekle")]
        public async Task<IActionResult> AddCategory()
        {
            string token = GetToken();
            var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories, token);
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost("createCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([FromForm] Category category)
        {
            string token = GetToken();

            var res = await _categoryRepo.Create(StaticDetail.StaticDetails.createCategory, category, token);

            return RedirectToAction("AddCategory");
        }

        //[Route(template:"getAllCategories", Name ="Kategori Listesi")]
        //public async Task<IActionResult> GetAllCategories()
        //{
        //    this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
        //    string token = string.Empty;
        //    if (value.Length > 0)
        //    {
        //        token = Encoding.Default.GetString(value);
        //    }
        //    var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories, token);
        //    return View(categories);
        //}

        [HttpGet("updateCategory/{Id}")]
        public async Task<IActionResult> UpdateCategory(int Id)
        {
            string token = GetToken();
            var category = await _categoryRepo.Get(StaticDetail.StaticDetails.getCategory + Id, token);
            return View(category);
        }

        [HttpPost("updateCategoryContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategoryContent([FromForm] Category category)
        {
            string token = GetToken();
            var res = await _categoryRepo.Update(StaticDetail.StaticDetails.updateCategory, category, token);
            return RedirectToAction("AddCategory");
        }

        [Route("deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            string token = GetToken();
            bool result = await _categoryRepo.Delete(StaticDetail.StaticDetails.deleteCategory + Id, token);
            if (result)
            {
                TempData["success"] = "Kategori silindi.  ";
            }
            else
            {
                TempData["fail"] = "Kategori silinirken bir hata oluştu";
            }

            return RedirectToAction("AddCategory");
        }
    }
}
