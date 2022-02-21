﻿using Microsoft.AspNetCore.Mvc;
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
    public class FeatureDescriptionController : Controller
    {
        private readonly IFeatureDescriptionRepo _featureDescriptionRepo;
        private readonly IFeatureRepo _featureRepo;

        public FeatureDescriptionController(IFeatureDescriptionRepo featureDescriptionRepo, IFeatureRepo featureRepo)
        {
            _featureDescriptionRepo = featureDescriptionRepo;
            _featureRepo = featureRepo;

        }

        [Route(template: "addFeatureDescription", Name = "Adedi Bilgi İçeriği")]
        public async Task<IActionResult> AddFeatureDescription()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getAllFeatures, token);
            List<SelectListItem> featureList = new();
            foreach (var item in features)
            {
                featureList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.FeatureList = featureList;

            var featureDescs = await _featureDescriptionRepo.GetList(StaticDetail.StaticDetails.getAllfeatureDescriptions, token);
            ViewBag.FeatureDescList = featureDescs;
            return View();
        }

        [HttpPost("createFeatureDescription")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeatureDescription([FromForm] FeatureDescription featureDescription)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _featureDescriptionRepo.Create(StaticDetail.StaticDetails.createfeatureDescription, featureDescription, token);

            return RedirectToAction("AddFeatureDescription");
        }

        //[Route(template: "getAllFeaturesDescription", Name = "Adedi Bilgi İçerik Listesi")]
        //public async Task<IActionResult> GetAllFeaturesDescription()
        //{
        //    this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
        //    string token = string.Empty;
        //    if (value.Length > 0)
        //    {
        //        token = Encoding.Default.GetString(value);
        //    }
        //    var featureDescs = await _featureDescriptionRepo.GetList(StaticDetail.StaticDetails.getAllfeatureDescriptions, token);
        //    return View(featureDescs);
        //}

        [HttpGet("updateFeatureDescription/{Id}")]
        public async Task<IActionResult> UpdateFeatureDescription(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var features = await _featureRepo.GetList(StaticDetail.StaticDetails.getAllFeatures);
            var fetureDescriptions = await _featureDescriptionRepo.Get(StaticDetail.StaticDetails.getfeatureDescription + Id, token);
            List<SelectListItem> featureList = new();
            foreach (var item in features)
            {
                featureList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == fetureDescriptions.FeatureId) });
            }
            ViewBag.FeatureList = featureList;
            return View(features);
        }

        [HttpPost("updateFeatureDescriptionContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFeatureDescriptionContent([FromForm] Feature feature)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var res = await _featureRepo.Update(StaticDetail.StaticDetails.updateFeature, feature, token);

            return RedirectToAction("AddFeatureDescription");
        }

        [Route("deleteFeatureDescription/{id}")]
        public async Task<IActionResult> DeleteFeatureDescription(int Id)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            bool result = await _featureRepo.Delete(StaticDetail.StaticDetails.deleteFeature + Id, token);
            if (result)
            {
                TempData["success"] = "Adedi Bilgi İçeriği silindi.  ";
            }
            else
            {
                TempData["fail"] = "Adedi Bilgi İçeriği silinirken bir hata oluştu";
            }

            return RedirectToAction("AddFeatureDescription");
        }
    }
}

