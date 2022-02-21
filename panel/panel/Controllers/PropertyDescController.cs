using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class PropertyDescController : Controller
    {
        private readonly IPropertyDescRepo _propertyTabRepo;
        private readonly IProductPropertyRepo _productPropertyRepo;
        public PropertyDescController(IPropertyDescRepo propertyTabRepo, IProductPropertyRepo productPropertyRepo)
        {
            _propertyTabRepo = propertyTabRepo;
            _productPropertyRepo = productPropertyRepo;
        }

        [Route(template:"addPropertyDescription", Name ="Özellik Açıklaması Ekle")]
        public async Task<IActionResult> AddPropertyDescription()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var result = await _productPropertyRepo.GetList(StaticDetail.StaticDetails.getAllProductProperties, token);
            List<SelectListItem> productPropDescList = new();
            foreach (var item in result)
            {
                productPropDescList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.ProductPropDescList = productPropDescList;
            return View();
        }

        [HttpPost("createPropertyDescription")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePropertyDescription([FromForm] PropertyDescription propertyDescription)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _propertyTabRepo.Create(StaticDetail.StaticDetails.createPropertyDesc, propertyDescription, token);

            return RedirectToAction("getAllPropertyDescription");
        }


        [Route(template:"getAllPropertyDescription", Name ="Özellik Açıklamaları Listesi")]
        public async Task<IActionResult> GetAllPropertyDescription()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var properties = await _propertyTabRepo.GetList(StaticDetail.StaticDetails.getAllPropertyDescs, token);
            foreach (var item in properties)
            {
                var y = await _propertyTabRepo.Get(StaticDetail.StaticDetails.getProductProperty + item.ProductPropertyId, token);
                item.ProductPropertyName = y.Name;
            }
            PropertyDescription propertyDes = new();
            return View(properties);
        }


        [HttpGet("updatePropertyDescription/{Id}")]
        public async Task<IActionResult> UpdatePropertyDescription(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var properties = await _productPropertyRepo.GetList(StaticDetail.StaticDetails.getAllProductProperties, token);
            var descritions = await _propertyTabRepo.Get(StaticDetail.StaticDetails.getPropertyDesc+ Id, token);
            List<SelectListItem> productPropertyList = new();
            foreach (var item in properties)
            {
                productPropertyList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = descritions.ProductPropertyId==item.Id ? true : false });
            }
            ViewBag.ProductPropertyList = productPropertyList;
            return View(descritions);
        }

        [HttpPost("updatePropertyDescriptionContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePropertyDescriptionContent([FromForm] PropertyDescription propertyDescription)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _propertyTabRepo.Update(StaticDetail.StaticDetails.updatePropertyDesc, propertyDescription, token);

            return RedirectToAction("getAllPropertyDescription");
        }

        [Route("deletePropertyDescription/{id}")]
        public async Task<IActionResult> DeletePropertyDescription(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _propertyTabRepo.Delete(StaticDetail.StaticDetails.deletePropertyDesc + Id, token);
            if (result)
            {
                TempData["success"] = "Özellik Açıklması silindi.  ";
            }
            else
            {
                TempData["fail"] = "Özellik Açıklması silinirken bir hata oluştu";
            }

            return RedirectToAction("getAllPropertyDescription");
        }
    }
}
