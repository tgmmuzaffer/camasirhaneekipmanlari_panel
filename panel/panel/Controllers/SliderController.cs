using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using panel.Models;
using panel.Models.Dtos;
using panel.RepoExtension;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class SliderController : Controller
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
            return View();
        }

        [HttpPost("createSlider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(SliderDto sliderDto)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            sliderDto.ImageName = ClearString.Clear(sliderDto.SliderName) + ".webp";
            var uploadedfilePath = await _fileUpload.UploadFile(sliderDto.Image, sliderDto.ImageName);
            if (!string.IsNullOrEmpty(uploadedfilePath))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                sliderDto.ImageData = Convert.ToBase64String(imageArray);
            }

            var result = await _sliderRepo.Create(StaticDetail.StaticDetails.createSlider, sliderDto, token);
            return RedirectToAction("GetAllSlider");
        }

        [Route(template: "getAllSlider", Name = "Slider Listesi")]
        public async Task<IActionResult> GetAllSlider()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var result =await _sliderRepo.GetList(StaticDetail.StaticDetails.getAllSliders, token);
            return View(result);
        }

        [HttpGet("updateSlider/{Id}")]
        public async Task<IActionResult> UpdateSlider(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var blog = await _sliderRepo.Get(StaticDetail.StaticDetails.getSlider + Id, token);
            return View(blog);
        }

        [HttpPost("updateSliderContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSliderContent([FromForm] SliderDto sliderDto)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            string uploadedfilePath = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            sliderDto.ImageName = ClearString.Clear(sliderDto.SliderName)+ ".webp";
            if (sliderDto.Image == null)
            {
                var orjsliderdetails = await _sliderRepo.Get(StaticDetail.StaticDetails.getSlider + sliderDto.Id, token);
                if ((sliderDto.ImageName + ".webp") == orjsliderdetails.ImageName)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjsliderdetails.ImageName;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    sliderDto.ImageData = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjsliderdetails.ImageName;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + sliderDto.ImageName;
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    sliderDto.ImageData = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
                uploadedfilePath = await _fileUpload.UploadFile(sliderDto.Image, sliderDto.ImageName);
                if (!string.IsNullOrEmpty(uploadedfilePath))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                    sliderDto.ImageData = Convert.ToBase64String(imageArray);
                }
            }

            var res = await _sliderRepo.Update(StaticDetail.StaticDetails.updateSlider, sliderDto, token);
            return RedirectToAction("GetAllSlider");
        }

        [Route("deleteSlider/{id}/{ImageName}")]
        public async Task<IActionResult> DeleteBlog(int Id, string ImageName)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            //string imgPath = ClearString.Clear(title);
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

            return RedirectToAction("GetAllSlider");
        }
    }
}
