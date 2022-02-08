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
using Microsoft.AspNetCore.Hosting;
using panel.Models.Dtos;

namespace panel.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IFileUpload _fileUpload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductRepo productRepo, IFileUpload fileUpload, ICategoryRepo categoryRepo, IHostingEnvironment hostingEnvironment)
        {
            _productRepo = productRepo;
            _fileUpload = fileUpload;
            _categoryRepo = categoryRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route(template: "addProduct", Name = "Ürün Ekle")]
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
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto product)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            product.ImagePath = StringProcess.ClearString(product.Name);
            product.ImageName = product.ImagePath;
            string uploadedfilePath = await _fileUpload.UploadFile(product.ImageFile, product.ImagePath + ".webp");
            if (!string.IsNullOrEmpty(uploadedfilePath))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                product.ImagePath = Convert.ToBase64String(imageArray);
            }
            var result = await _productRepo.Create(StaticDetail.StaticDetails.createProduct, product, token);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet(template: "getAllProducts", Name = "Ürün Listesi")]
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
        public async Task<IActionResult> UpdateProduct(int Id)
        {
            var product = await _productRepo.Get(StaticDetail.StaticDetails.getProduct + Id);
            var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories);
            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var item in categories)
            {
                categoryList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (product.CategoryId == (item.Id))
                });
            }
            ViewBag.Categories = categoryList;
            return View(product);
        }

        [HttpPost("updateProductContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProductContent([FromForm] ProductDto productDto)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            string uploadedfilePath = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            productDto.ImageName = StringProcess.ClearString(productDto.Name);
            if (productDto.ImageFile == null)
            {
                var orjproductdetails = await _productRepo.Get(StaticDetail.StaticDetails.getProduct + productDto.Id, token);
                if ((productDto.ImageName + ".webp") == orjproductdetails.ImagePath)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjproductdetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    productDto.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjproductdetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + productDto.ImageName + ".webp";
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    productDto.ImagePath = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
                uploadedfilePath = await _fileUpload.UploadFile(productDto.ImageFile, productDto.ImageName);
                if (!string.IsNullOrEmpty(uploadedfilePath))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                    productDto.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    productDto.ImageName = string.Empty;
                }
            }

            var res = await _productRepo.Update(StaticDetail.StaticDetails.updateProduct, productDto, token);
            return RedirectToAction("getAllProducts");
        }

        [Route("deleteProduct/{id}")]
        [Route("deleteProduct/{id}/{title}")]
        public async Task<IActionResult> DeleteCategory(int Id, string? title)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            //string imgPath = StringProcess.ClearString(title);
            bool result = await _categoryRepo.Delete(StaticDetail.StaticDetails.deleteProduct + Id, token);
            if (!string.IsNullOrEmpty(title))
            {
                var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + title;
                System.IO.File.Delete(orjpath);
            }
          

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
