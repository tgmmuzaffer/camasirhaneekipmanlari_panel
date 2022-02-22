using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class FeatureController : BaseController
    {
        private readonly ISubCategoryRepo _subCategoryRepo;
        private readonly IFeatureRepo _featureRepo;
        private readonly IFe_SubCat_RelRepo _fe_SubCat_RelRepo;

        public FeatureController(ISubCategoryRepo subCategoryRepo, IFeatureRepo featureRepo, IFe_SubCat_RelRepo fe_SubCat_RelRepo)
        {
            _subCategoryRepo = subCategoryRepo;
            _featureRepo = featureRepo;
            _fe_SubCat_RelRepo = fe_SubCat_RelRepo;
        }

        [Route(template: "addFeature", Name = "Adedi Bilgi Ekle")]
        public async Task<IActionResult> AddFeature()
        {
            string token = GetToken();
            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getAllFeatures, token);
            ViewBag.FeatureList = features;
            return View();
        }

        [HttpPost("createFeature")]
        public async Task<IActionResult> CreateFeature(string featureName)
        {
            string token = GetToken();
            var res = await _featureRepo.UpdateCreateFeature(StaticDetail.StaticDetails.createFeature, new Feature { Name = featureName }, token);
            if (res != null && res.Any(a => a.Name == featureName))
            {
                TempData["success"] = "Adedi Bilgi Eklendi.  ";
                var jsonRes = JsonConvert.SerializeObject(res);
                return Json(jsonRes);
            }

            TempData["fail"] = "Adedi Bilgi Eklenirken bir hata oluştu";
            return Json(0);
        }

        [Route(template: "getAllFeatures", Name = "Adedi Bilgi Listesi")]
        public async Task<IActionResult> GetAllFeatures()
        {
            string token = GetToken();
            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getAllFeatures, token);
            return View(features);
        }

        [Route(template: "linkFeaturesToSubCat", Name = "Adedi Bilgi İlişkilendir")]
        public async Task<IActionResult> LinkFeaturesToSubCat()
        {
            string token = GetToken();
            var subcategories = await _subCategoryRepo.GetList(StaticDetail.StaticDetails.getAllSubCategories, token);
            List<SelectListItem> subCategoryList = new();
            foreach (var item in subcategories)
            {
                subCategoryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.SubCategoryList = subCategoryList;
            return View();
        }

        [HttpPost("updateCreateFeatureSubCatLinks")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCreateFeatureSubCatLinks(Feature feature)
        {
            string token = GetToken();
            var featureIds = feature.FeatureIds.Split(",");
            List<int> fIds = new();
            foreach (var item in featureIds)
            {
                fIds.Add(Convert.ToInt32(item));
            }

            List<Fe_SubCat_Relational> fe_SubCat_Relationals = new();
            foreach (var item in fIds)
            {
                Fe_SubCat_Relational fe_SubCat_Relational = new()
                {
                    FeatureId = Convert.ToInt32(item),
                    SubCategoryId = feature.SubCategoryId
                };
                fe_SubCat_Relationals.Add(fe_SubCat_Relational);
            }
            var res = await _fe_SubCat_RelRepo.UpdateCreateFeatureSubCatLinks(StaticDetail.StaticDetails.createupdateFeSubCat, fe_SubCat_Relationals, token);
            if (res)
            {
                TempData["success"] = "Adedi Bilgi Eklendi.  ";
                return RedirectToAction("LinkFeaturesToSubCat");
            }


            TempData["fail"] = "Adedi Bilgi Eklenirken bir hata oluştu";
            return RedirectToAction("LinkFeaturesToSubCat");

        }


        [Route(template: "getFeaturesBySubCatId/{Id}")]
        public async Task<IActionResult> GetAllFeaturesBySubCatId(int Id)
        {
            string token = GetToken();
            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getFeaturesBySubCatId + Id, token);
            var fe_subCatRel = await _fe_SubCat_RelRepo.GetList(StaticDetail.StaticDetails.getAllFeSubCats, token);
            var rel_IdList = fe_subCatRel.Where(a => a.SubCategoryId == Id).Select(b => b.FeatureId).ToList();
            foreach (var item in features)
            {
                if (rel_IdList.Any(a => a == item.Id))
                {
                    item.IsChoosen = true;
                }
                else
                {
                    item.IsChoosen = false;
                }
            }

            if (features.Count == 0)
            {
                return Json(0);
            }

            return Json(features);
        }

        [HttpGet("updateFeature/{Id}")]
        public async Task<IActionResult> UpdateFeature(int Id)
        {
            string token = GetToken();
            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getAllFeatures, token);
            var feature = features.FirstOrDefault(a => a.Id == Id);
            features = features.Where(b => b.Id != Id).ToList();
            //var subCategories = await _subCategoryRepo.GetList(StaticDetail.StaticDetails.getAllSubCategories, token);
            //List<SelectListItem> subCategorylist = new();
            //foreach (var item in subCategories)
            //{
            //    subCategorylist.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id==feature.SubCategories.FirstOrDefault(a=>a.Id)) });
            //}
            //ViewBag.SubCategorylist = subCategorylist;
            ViewBag.Feature = feature;
            return View(features);
        }


        [HttpPost("updateFeatureContent")]
        public async Task<IActionResult> UpdateFeatureContent(int inptid, string name)
        {
            string token = GetToken();
            var res = await _featureRepo.UpdateCreateFeature(StaticDetail.StaticDetails.updateFeature, new Feature { Id = inptid, Name = name }, token);
            if (res != null)
            {
                TempData["success"] = "Adedi Bilgi Güncellendi.  ";
                var jsonRes = JsonConvert.SerializeObject(res);
                return Json(jsonRes);
            }

            TempData["fail"] = "Adedi Bilgi Güncelenirken bir hata oluştu";
            return Json(0);
        }

        [Route("deleteFeature/{id}")]
        public async Task<IActionResult> DeleteFeature(int Id)
        {
            string token = GetToken();
            bool result = await _featureRepo.Delete(StaticDetail.StaticDetails.deleteFeature + Id, token);
            if (result)
            {
                TempData["success"] = "Adedi Bilgi silindi.  ";
            }
            else
            {
                TempData["fail"] = "Adedi Bilgi silinirken bir hata oluştu";
            }

            return RedirectToAction("AddFeature");
        }
    }
}
