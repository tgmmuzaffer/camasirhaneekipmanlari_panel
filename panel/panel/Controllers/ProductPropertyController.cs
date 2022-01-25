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
    public class ProductPropertyController : Controller
    {
        private readonly IProductPropertyRepo _productPropertyRepo;
        public ProductPropertyController(IProductPropertyRepo productPropertyRepo)
        {
            _productPropertyRepo = productPropertyRepo;
        }

        [Route(template:"addProductProperty", Name ="Özellik Ekle")]
        public IActionResult AddProductProperty()
        {
            return View();
        }

        [HttpPost("createProductProperty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductProperty([FromForm] ProductProperty productProperty)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            { 
                token = Encoding.Default.GetString(value);
            }
            var res = await _productPropertyRepo.Create(StaticDetail.StaticDetails.createProductProperty, productProperty, token);
            return RedirectToAction("GetAllProductProperties");
        }

        [Route(template:"getAllProductProperties", Name ="Özellik Listesi")]
        public async Task<IActionResult> GetAllProductProperties()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var result =await _productPropertyRepo.GetList(StaticDetail.StaticDetails.getAllProductProperties, token);
            return View(result);
        }

        [HttpGet("updateProductProperty/{Id}")]
        public async Task<IActionResult> UpdateProductProperty(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var productProperty = await _productPropertyRepo.Get(StaticDetail.StaticDetails.getProductProperty + Id, token);
            return View(productProperty);
        }

        [HttpPost("updateProductPropertyContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProductPropertyContent([FromForm] ProductProperty productProperty)
        {

            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var result = await _productPropertyRepo.Update(StaticDetail.StaticDetails.updateProductProperty, productProperty, token);
            if (result)
            {
                TempData["success"] = "Özellik güncellendi.  ";
            }
            else
            {
                TempData["fail"] = "Özellik güncellenirken bir hata oluştu";
            }

            return RedirectToAction("GetAllProductProperties");
        }

        [Route("deleteProductProperty/{id}")]
        public async Task<IActionResult> DeleteProductProperty(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _productPropertyRepo.Delete(StaticDetail.StaticDetails.deleteProductProperty + Id, token) ;
            if (result)
            {
                TempData["success"] = "Özellik silindi.  ";
            }
            else
            {
                TempData["fail"] = "Özellik silinirken bir hata oluştu";
            }

            return RedirectToAction("GetAllProductProperties");
        }
    }
}
