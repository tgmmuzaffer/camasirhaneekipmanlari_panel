using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Extensions;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISubCategoryRepo _subCategoryRepo;
        private readonly IFileUpload _fileUpload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CategoryController(ICategoryRepo categoryRepo, ISubCategoryRepo subCategoryRepo, IFileUpload fileUpload, IHostingEnvironment hostingEnvironment)
        {
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;
            _fileUpload = fileUpload;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route(template: "addCategory", Name = "Kategori Ekle")]
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
            category.ImageName = StringProcess.ClearString(category.Name);
            string[] uploadedfilePath = Array.Empty<string>();
            if (category.ImageFile != null)
            {
                uploadedfilePath = await _fileUpload.UploadFile(category.ImageFile, category.ImageName);
            }

            if (uploadedfilePath.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                category.ImagePath = Convert.ToBase64String(imageArray);
                category.ImageName = uploadedfilePath[1];
            }

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
        [HttpGet("getCategoryName/{Id}")]
        public async Task<IActionResult> GetCategoryName(int Id)
        {
            var category = await _categoryRepo.Get(StaticDetail.StaticDetails.getCategoryName + Id);
            return View(category);
        }

        [HttpGet("getCategoryByName/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var category = await _categoryRepo.Get(StaticDetail.StaticDetails.getCategoryName + name);
            return View(category);
        }

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
            category.ImageName = StringProcess.ClearString(category.Name) + ".webp";
            if (category.ImageFile == null)
            {
                var orjcategorydetails = await _categoryRepo.Get(StaticDetail.StaticDetails.getCategoryName + category.Id);
                if (category.ImageName == orjcategorydetails.ImagePath)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjcategorydetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    category.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjcategorydetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + category.ImageName;
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    category.ImagePath = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
                string[] uploadedfilePath = await _fileUpload.UploadFile(category.ImageFile, StringProcess.ClearString(category.Name));
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    category.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    category.ImageName = string.Empty;
                }
            }
            var res = await _categoryRepo.Update(StaticDetail.StaticDetails.updateCategory, category, token);
            return RedirectToAction("AddCategory");
        }

        [Route("deleteCategory/{id}")]
        [Route("deleteCategory/{id}/{title}")]
        public async Task<IActionResult> DeleteCategory(int Id, string? title)
        {
            string token = GetToken();
            bool result = await _categoryRepo.Delete(StaticDetail.StaticDetails.deleteCategory + Id, token);
            if (!string.IsNullOrEmpty(title))
            {
                var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + title;
                System.IO.File.Delete(orjpath);
            }
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
