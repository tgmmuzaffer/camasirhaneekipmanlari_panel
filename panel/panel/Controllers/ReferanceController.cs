﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using panel.Models.Dtos;
using panel.Extensions;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class ReferanceController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IReferanceRepo _referanceRepo;
        private readonly IFileUpload _fileUpload;
        public ReferanceController(IHostingEnvironment hostingEnvironment, IReferanceRepo referanceRepo, IFileUpload fileUpload)
        {
            _hostingEnvironment = hostingEnvironment;
            _referanceRepo = referanceRepo;
            _fileUpload = fileUpload;
        }
        [Route(template: "addReferance", Name = "Referans Ekle")]
        public async Task<IActionResult> AddReferance()
        {
            return View();
        }

        [HttpPost("createReferance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReferance(ReferanceDto referanceDto)
        {
            string token = GetToken();
            referanceDto.ImageName = StringProcess.ClearString(referanceDto.Name);
            string[] uploadedfilePath = await _fileUpload.UploadFile(referanceDto.Image, referanceDto.ImageName);
            if (uploadedfilePath.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                referanceDto.ImageData = Convert.ToBase64String(imageArray);
            }

            var result = await _referanceRepo.Create(StaticDetail.StaticDetails.createReferance, referanceDto, token);
            return RedirectToAction("GetAllReferance");
        }

        [Route(template: "getAllReferance", Name = "Referans Listesi")]
        public async Task<IActionResult> GetAllReferance()
        {
            string token = GetToken();
            var result = await _referanceRepo.GetList(StaticDetail.StaticDetails.getAllReferances, token);
            return View(result);
        }

        [HttpGet("updateReferance/{Id}")]
        public async Task<IActionResult> UpdateReferance(int Id)
        {
            string token = GetToken();
            var result = await _referanceRepo.Get(StaticDetail.StaticDetails.getReferance + Id, token);
            return View(result);
        }

        [HttpPost("updateReferanceContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReferanceContent([FromForm] ReferanceDto referanceDto)
        {
            string token = GetToken();
            referanceDto.ImageName = StringProcess.ClearString(referanceDto.Name) + ".webp";
            if (referanceDto.Image == null)
            {
                var orjreferancedetails = await _referanceRepo.Get(StaticDetail.StaticDetails.getReferance + referanceDto.Id, token);
                if ((referanceDto.ImageName + ".webp") == orjreferancedetails.ImageName)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjreferancedetails.ImageName;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    referanceDto.ImageData = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjreferancedetails.ImageName;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + referanceDto.ImageName;
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    referanceDto.ImageData = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
                string[] uploadedfilePath = await _fileUpload.UploadFile(referanceDto.Image, referanceDto.ImageName);
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    referanceDto.ImageData = Convert.ToBase64String(imageArray);
                }
            }

            var res = await _referanceRepo.Update(StaticDetail.StaticDetails.updateReferance, referanceDto, token);
            return RedirectToAction("GetAllReferance");
        }

        [Route("deleteReferance/{id}/{ImageName}")]
        public async Task<IActionResult> DeleteReferance(int Id, string ImageName)
        {
            string token = GetToken();
            bool result = await _referanceRepo.Delete(StaticDetail.StaticDetails.deleteReferance + Id, token);
            var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + ImageName;
            System.IO.File.Delete(orjpath);
            if (result)
            {
                TempData["success"] = "Referans silindi.  ";
            }
            else
            {
                TempData["fail"] = "Referans silinirken bir hata oluştu";
            }

            return RedirectToAction("GetAllReferance");
        }
    }
}
