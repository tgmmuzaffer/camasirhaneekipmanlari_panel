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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IProductPropertyRepo _productPropertyRepo;

        public CategoryController(ICategoryRepo categoryRepo, IProductPropertyRepo productPropertyRepo)
        {
            _categoryRepo = categoryRepo;
            _productPropertyRepo = productPropertyRepo;

        }

        [Route(template:"addCategory", Name ="Kategori Ekle")]
        public async Task<IActionResult> AddCategory()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var result = await _productPropertyRepo.GetList(StaticDetail.StaticDetails.getAllProductProperties, token);
            List<SelectListItem> productPropertyList = new List<SelectListItem>();
            foreach (var item in result)
            {
                productPropertyList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.ProductPropertyList = productPropertyList;
            return View();
        }

        [HttpPost("createCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDto category)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _categoryRepo.Create(StaticDetail.StaticDetails.createCategory, category, token);

            return RedirectToAction("GetAllCategories");
        }
        [Route(template:"getAllCategories", Name ="Kategori Listesi")]
        public async Task<IActionResult> GetAllCategories()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories, token);
            return View(categories);
        }

        [HttpGet("updateCategory/{Id}")]
        public async Task<IActionResult> UpdateCategory(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var category = await _categoryRepo.Get(StaticDetail.StaticDetails.getCategory + Id, token);
            var result = await _productPropertyRepo.GetList(StaticDetail.StaticDetails.getAllProductProperties, token);
            List<SelectListItem> productPropertyList = new();
            foreach (var item in result)
            {
                //var a = category.ProductPropertyIds.Contains(item.Id);
                productPropertyList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (category.ProductPropertyIds.Contains(item.Id) ? true : false) });
            }
            ViewBag.ProductPropertyList = productPropertyList;
            return View(category);
        }

        [HttpPost("updateCategoryContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategoryContent([FromForm] CategoryDto categoryDto)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _categoryRepo.Update(StaticDetail.StaticDetails.updateCategory, categoryDto, token);

            return RedirectToAction("GetAllCategories");
        }

        [Route("deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _categoryRepo.Delete(StaticDetail.StaticDetails.deleteCategory + Id, token);
            if (result)
            {
                TempData["success"] = "Kategori silindi.  ";
            }
            else
            {
                TempData["fail"] = "Kategori silinirken bir hata oluştu";
            }

            return RedirectToAction("getAllCategories");
        }
    }
}
