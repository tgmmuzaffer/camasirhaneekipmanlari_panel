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
    public class ProductController : BaseController
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IFeatureRepo  _featureRepo;
        private readonly IFeatureDescriptionRepo _featureDescriptionRepo;
        private readonly IPr_Fe_RelRepo _pr_Fe_RelRepo;
        private readonly IFe_SubCat_RelRepo _fe_SubCat_RelRepo;
        private readonly IFileUpload _fileUpload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductRepo productRepo, IFeatureRepo featureRepo, IFeatureDescriptionRepo featureDescriptionRepo, IFe_SubCat_RelRepo fe_SubCat_RelRepo, IFileUpload fileUpload, ICategoryRepo categoryRepo, IHostingEnvironment hostingEnvironment)
        {
            _productRepo = productRepo;
            _featureRepo = featureRepo;
            _featureDescriptionRepo = featureDescriptionRepo;
            _fileUpload = fileUpload;
            _categoryRepo = categoryRepo;
            _fe_SubCat_RelRepo = fe_SubCat_RelRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route(template: "addProduct", Name = "Ürün Ekle")]
        public async Task<IActionResult> AddProduct()
        {
            string token = GetToken();
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
            string token = GetToken();
            product.ImageName = StringProcess.ClearString(product.Name);
            string[] uploadedfilePath = await _fileUpload.UploadFile(product.ImageFile, product.ImageName);
            if (uploadedfilePath.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                product.ImagePath = Convert.ToBase64String(imageArray);
            }

            if (!string.IsNullOrEmpty(product.FeatureIds) || !string.IsNullOrEmpty(product.FeatureDescriptionIds))
            {
                var prFeIds = product.FeatureIds.Split(",").ToList();
                var prFeDescId = product.FeatureDescriptionIds.Split(",").ToList();
                List<Feature> featureList = new();
                List<FeatureDescription> featureDescriptionList = new();
                foreach (var itemprFe in prFeIds)
                {
                    Feature feature = new() { Id = Convert.ToInt32(itemprFe) };
                    featureList.Add(feature);                    
                }
                foreach (var itemprFeDesc in prFeDescId)
                {
                    FeatureDescription featureDescription = new() { Id = Convert.ToInt32(itemprFeDesc) };
                    featureDescriptionList.Add(featureDescription);                    
                }

                product.Feature = featureList;
                product.FeatureDescriptions = featureDescriptionList;
                var prodresult = await _productRepo.Create(StaticDetail.StaticDetails.createProduct, product, token);
                if (prodresult <= 0)
                {
                    TempData["fail"] = "Ürün eklenirken bir hata oluştu";
                    return RedirectToAction("GetAllProducts");
                }

                return RedirectToAction("GetAllProducts");
            }
            else
            {
                var prodresult = await _productRepo.Create(StaticDetail.StaticDetails.createProduct, product, token);
                if (prodresult <= 0)
                {
                    TempData["fail"] = "Ürün eklenirken bir hata oluştu";
                    return RedirectToAction("GetAllProducts");
                }
            }

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet(template: "getAllProducts", Name = "Ürün Listesi")]
        public async Task<IActionResult> GetAllProducts()
        {
            string token = GetToken();
            var products = await _productRepo.GetList(StaticDetail.StaticDetails.getAllProducts, token);
            return View(products);
        }

        [HttpGet("updateProduct/{Id}")]
        public async Task<IActionResult> UpdateProduct(int Id)
        {
            var product = await _productRepo.Get(StaticDetail.StaticDetails.getProduct + Id);
            var categories = await _categoryRepo.GetList(StaticDetail.StaticDetails.getAllCategories);
            List<SelectListItem> categoryList = new();
            foreach (var item in categories)
            {
                categoryList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (product.CategoryId == (item.Id))
                });
            }

            List<SelectListItem> subcategoryList = new();
            foreach (var itemSubCat in product.Category.SubCategories)
            {
                subcategoryList.Add(new SelectListItem() {
                    Text =itemSubCat.Name,
                    Value=itemSubCat.Id.ToString(),
                    Selected = (product.SubCategoryId == itemSubCat.Id)
                });
            }

            List<Feature> featureList = new();
            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getFeaturesBySubCatId + Id);
            var fe_subCatRel = await _fe_SubCat_RelRepo.GetList(StaticDetail.StaticDetails.getAllFeSubCats);
            var rel_IdList = fe_subCatRel.Where(a => a.SubCategoryId == product.SubCategoryId).Select(b => b.FeatureId).ToList();
            foreach (var item in features)
            {
                if (rel_IdList.Any(a => a == item.Id))
                {
                    featureList.Add(item);
                }
            }
            ViewBag.Feature = featureList;
            ViewBag.Categories = categoryList;
            ViewBag.SubCategories = subcategoryList;
            return View(product);
        }

        [HttpPost("updateProductContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProductContent([FromForm] Product product)
        {
            string token = GetToken();
            product.ImageName = StringProcess.ClearString(product.Name);
            if (product.ImageFile == null)
            {
                var orjproductdetails = await _productRepo.Get(StaticDetail.StaticDetails.getProduct + product.Id, token);
                if ((product.ImageName + ".webp") == orjproductdetails.ImagePath)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjproductdetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    product.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjproductdetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + product.ImageName + ".webp";
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    product.ImagePath = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
               string[] uploadedfilePath = await _fileUpload.UploadFile(product.ImageFile, product.ImageName + ".webp");
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    product.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    product.ImageName = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(product.FeatureIds) || !string.IsNullOrEmpty(product.FeatureDescriptionIds))
            {
                var prFeIds = product.FeatureIds.Split(",").ToList();
                var prFeDescId = product.FeatureDescriptionIds.Split(",").ToList();
                List<Feature> featureList = new();
                List<FeatureDescription> featureDescriptionList = new();
                foreach (var itemprFe in prFeIds)
                {
                    Feature feature = new() { Id = Convert.ToInt32(itemprFe) };
                    featureList.Add(feature);
                }
                foreach (var itemprFeDesc in prFeDescId)
                {
                    FeatureDescription featureDescription = new() { Id = Convert.ToInt32(itemprFeDesc) };
                    featureDescriptionList.Add(featureDescription);
                }

                product.Feature = featureList;
                product.FeatureDescriptions = featureDescriptionList;
                var prodresult = await _productRepo.Update(StaticDetail.StaticDetails.updateProduct, product, token);
                if (!prodresult)
                {
                    TempData["fail"] = "Ürün güncellenirken bir hata oluştu";
                    return RedirectToAction("GetAllProducts");
                }

                return RedirectToAction("GetAllProducts");
            }
            else
            {
                var prodresult = await _productRepo.Update(StaticDetail.StaticDetails.updateProduct, product, token);
                if (prodresult)
                {
                    TempData["fail"] = "Ürün eklenirken bir hata oluştu";
                    return RedirectToAction("GetAllProducts");
                }
            }

            return RedirectToAction("getAllProducts");
        }

        [Route("deleteProduct/{id}")]
        [Route("deleteProduct/{id}/{title}")]
        public async Task<IActionResult> DeleteCategory(int Id, string? title)
        {
            string token = GetToken();
            bool result = await _productRepo.Delete(StaticDetail.StaticDetails.deleteProduct + Id, token);
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
