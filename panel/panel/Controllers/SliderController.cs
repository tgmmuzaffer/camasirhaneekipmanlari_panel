using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using panel.Models;
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
    public class SliderController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISliderRepo _sliderRepo;
        private readonly IFileUpload _fileUpload;
        public SliderController(IHostingEnvironment hostingEnvironment, ISliderRepo sliderRepo, IFileUpload fileUpload)
        {
            _hostingEnvironment = hostingEnvironment;
            _sliderRepo = sliderRepo;
            _fileUpload = fileUpload;
        }

        [Route(template: "addSlider", Name = "Slider Ekle")]
        public async Task<IActionResult> AddSlider()
        {
            string token = GetToken();
            var sliders = await _sliderRepo.GetList(StaticDetail.StaticDetails.getAllSliders, token);
            ViewBag.Sliders = sliders;
            return View();
        }

        [HttpPost("createSlider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(Slider slider)
        {
            string token = GetToken();
            slider.ImageName = StringProcess.ClearString(slider.SliderName);
            string[] uploadedfilePath = Array.Empty<string>();
            if (slider.Image != null)
            {
                uploadedfilePath = await _fileUpload.UploadFile(slider.Image, slider.ImageName, isblog:false, isslider:true);
            }

            if (uploadedfilePath.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                slider.ImageData = Convert.ToBase64String(imageArray);
                slider.ImageName = uploadedfilePath[1];
            }

            var result = await _sliderRepo.Create(StaticDetail.StaticDetails.createSlider, slider, token);
            return RedirectToAction("AddSlider");
        }

        //[Route(template: "getAllSlider", Name = "Slider Listesi")]
        //public async Task<IActionResult> GetAllSlider()
        //{
        //    string token = GetToken();
        //    var result =await _sliderRepo.GetList(StaticDetail.StaticDetails.getAllSliders, token);
        //    return View(result);
        //}

        [HttpGet("updateSlider/{Id}")]
        public async Task<IActionResult> UpdateSlider(int Id)
        {
            string token = GetToken();
            var blog = await _sliderRepo.Get(StaticDetail.StaticDetails.getSlider + Id, token);
            return View(blog);
        }

        [HttpPost("updateSliderContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSliderContent([FromForm] Slider slider)
        {
            string token = GetToken();
            slider.ImageName = StringProcess.ClearString(slider.SliderName)+ ".webp";
            if (slider.Image == null)
            {
                var orjsliderdetails = await _sliderRepo.Get(StaticDetail.StaticDetails.getSlider + slider.Id, token);
                if ((slider.ImageName + ".webp") == orjsliderdetails.ImageName)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjsliderdetails.ImageName;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    slider.ImageData = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjsliderdetails.ImageName;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + slider.ImageName;
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    slider.ImageData = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
                string[] uploadedfilePath = await _fileUpload.UploadFile(slider.Image, slider.ImageName, isblog: false, isslider:true);
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    slider.ImageData = Convert.ToBase64String(imageArray);
                }
                else
                {
                    slider.ImageName = string.Empty;
                }
            }

            var res = await _sliderRepo.Update(StaticDetail.StaticDetails.updateSlider, slider, token);
            return RedirectToAction("AddSlider");
        }

        [Route("deleteSlider/{id}")]
        [Route("deleteSlider/{id}/{ImageName}")]
        public async Task<IActionResult> DeleteSlider(int Id, string? ImageName)
        {
            string token = GetToken();
            bool result = await _sliderRepo.Delete(StaticDetail.StaticDetails.deleteSlider + Id, token);
            var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + ImageName;
            System.IO.File.Delete(orjpath);
            if (result)
            {
                TempData["success"] = "Slider silindi.  ";
            }
            else
            {
                TempData["fail"] = "Slider silinirken bir hata oluştu";
            }

            return RedirectToAction("AddSlider");
        }
    }
}
