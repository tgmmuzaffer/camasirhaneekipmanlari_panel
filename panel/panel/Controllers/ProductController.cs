using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Extensions;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IFileUpload _fileUpload;
        public ProductController(IProductRepo productRepo, IFileUpload fileUpload, ICategoryRepo categoryRepo)
        {
            _productRepo = productRepo;
            _fileUpload = fileUpload;
            _categoryRepo = categoryRepo;
        }

        [Route(template:"addProduct", Name ="Ürün Ekle")]
        public async Task<IActionResult> AddProduct()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories, token);

            List<SelectListItem> categorylist = new List<SelectListItem>();
            foreach (var item in categories)
            {
                categorylist.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.Categories = categorylist;
            return View();
        }

        [HttpPost("createProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([FromForm] Product product)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            product.ImagePath = StringProcess.ClearString(product.Name);
            product.ImageName = product.ImagePath;
            string uploadedfilePath =await _fileUpload.UploadFile(product.Imagefile, product.ImagePath);
            if (!string.IsNullOrEmpty(uploadedfilePath))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                product.ImagePath = Convert.ToBase64String(imageArray);
            }
            var result = await _productRepo.Create(StaticDetail.StaticDetails.createProduct, product, token);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet(template:"getAllProducts", Name ="Ürün Listesi")]
        public async Task<IActionResult> GetAllProducts()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var products = await _productRepo.GetList(StaticDetail.StaticDetails.getAllProducts, token);
            return View(products);
        }

        [HttpGet("updateProduct/{Id}")]
        public async Task<IActionResult> UpdateCategory(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var category = await _categoryRepo.Get(StaticDetail.StaticDetails.getCategory + Id, token);
            return View(category);
        }

        [HttpPost("updateProductContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProductContent(int Id)
        {
            return RedirectToAction("getAllProducts");
        }

        [Route("deleteProduct/{id}")]
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
                TempData["success"] = "Ürün silindi.  ";
            }
            else
            {
                TempData["fail"] = "Ürün silinirken bir hata oluştu";
            }

            return RedirectToAction("getAllProducts");
        }
    }
}
