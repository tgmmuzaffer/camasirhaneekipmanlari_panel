using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class SubCategoryController : BaseController
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISubCategoryRepo _subCategoryRepo;

        public SubCategoryController(ICategoryRepo categoryRepo, ISubCategoryRepo subCategoryRepo)
        {
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;

        }

        [Route(template: "addSubCategory", Name = "Alt Kategori Ekle")]
        public async Task<IActionResult> AddSubCategory()
        {
            string token = GetToken();
            var result = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories, token);
            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var item in result)
            {
                categoryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.CategoryList = categoryList;

            var subcategories = await _subCategoryRepo.GetList(StaticDetail.StaticDetails.getAllSubCategories, token);
            ViewBag.Subcategories = subcategories;

            return View();
        }

        [HttpPost("createSubCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory([FromForm] SubCategory subcategory)
        {
            string token = GetToken();
            var res = await _subCategoryRepo.Create(StaticDetail.StaticDetails.createSubCategory, subcategory, token);
            return RedirectToAction("AddSubCategory");
        }
        //[Route(template: "getAllSubCategories", Name = "Alt Kategori Listesi")]
        //public async Task<IActionResult> GetAllSubCategories()
        //{
        //    this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
        //    string token = string.Empty;
        //    if (value.Length > 0)
        //    {
        //        token = Encoding.Default.GetString(value);
        //    }
        //    var subcategories = await _subCategoryRepo.GetList(StaticDetail.StaticDetails.getAllSubCategories, token);
        //    return View(subcategories);
        //}

        [Route("getSubCategoryByCatId/{Id}")]
        public async Task<IActionResult> GetSubCategory(int Id)
        {
            string token = GetToken();
            var subcategory = await _subCategoryRepo.GetList(StaticDetail.StaticDetails.getSubCategoryByCatId + Id, token);            
            return Json(subcategory);
        }

        [HttpGet("updateSubCategory/{Id}")]
        public async Task<IActionResult> UpdateSubCategory(int Id)
        {
            string token = GetToken();
            var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories, token);
            var subcategory = await _subCategoryRepo.Get(StaticDetail.StaticDetails.getSubCategory + Id, token);
            List<SelectListItem> categoryList = new();
            foreach (var item in categories)
            {
                categoryList.Add(new SelectListItem()
                { 
                    Text = item.Name,
                    Value = item.Id.ToString(), 
                    Selected = item.Id==subcategory.CategoryId
                });
            }
            ViewBag.CategoryList = categoryList;
            return View(subcategory);
        }

        [HttpPost("updateSubCategoryContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSubCategoryContent([FromForm] SubCategory subCategory)
        {
            string token = GetToken();
            var res = await _subCategoryRepo.Update(StaticDetail.StaticDetails.updateSubCategory, subCategory, token);

            return RedirectToAction("AddSubCategory");
        }

        [Route("deleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int Id)
        {
            string token = GetToken();
            bool result = await _subCategoryRepo.Delete(StaticDetail.StaticDetails.deleteSubCategory + Id, token);
            if (result)
            {
                TempData["success"] = "Alt Kategori silindi.  ";
            }
            else
            {
                TempData["fail"] = "Alt Kategori silinirken bir hata oluştu";
            }

            return RedirectToAction("AddSubCategory");
        }
    }
}
