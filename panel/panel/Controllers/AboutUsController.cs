using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using panel.Extensions;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class AboutUsController : BaseController
    {
        private readonly IAboutUsRepo _aboutUsRepo;
        private readonly IFileUpload _fileUpload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AboutUsController(IAboutUsRepo aboutUsRepo, IFileUpload fileUpload, IHostingEnvironment hostingEnvironment)
        {
            _aboutUsRepo = aboutUsRepo;
            _fileUpload = fileUpload;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route(template: "addAboutUs", Name = "Hakkimizda Ekle")]
        public async Task<IActionResult> AddAboutUs()
        {
            var aboutus = await _aboutUsRepo.Get(StaticDetail.StaticDetails.getAboutUs);
            if (aboutus == null)
            {
                return View();
            }

            return RedirectToAction("UpdateAboutUs");
        }

        [HttpPost("createAboutUs")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAboutUs([FromForm] AboutUs aboutUs)
        {
            string token = GetToken();
            #region ImageUpload

            aboutUs.ImageName1 = StringProcess.ClearString("content1" + aboutUs.Title);
            string[] uploadedfilePath1 = Array.Empty<string>();
            if (aboutUs.ImageFile1 != null)
            {
                uploadedfilePath1 = await _fileUpload.UploadFile(aboutUs.ImageFile1, aboutUs.ImageName1);
            }

            if (uploadedfilePath1.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath1[0]);
                aboutUs.ImagePath1 = Convert.ToBase64String(imageArray);
                aboutUs.ImageName1 = uploadedfilePath1[1];
            }

            aboutUs.ImageName2 = StringProcess.ClearString("content2" + aboutUs.Title);
            string[] uploadedfilePath2 = Array.Empty<string>();
            if (aboutUs.ImageFile2 != null)
            {
                uploadedfilePath2 = await _fileUpload.UploadFile(aboutUs.ImageFile2, aboutUs.ImageName2);
            }

            if (uploadedfilePath2.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath2[0]);
                aboutUs.ImagePath2 = Convert.ToBase64String(imageArray);
                aboutUs.ImageName2 = uploadedfilePath2[1];
            }

            aboutUs.ImageName3 = StringProcess.ClearString("content3" + aboutUs.Title);
            string[] uploadedfilePath3 = Array.Empty<string>();
            if (aboutUs.ImageFile3 != null)
            {
                uploadedfilePath3 = await _fileUpload.UploadFile(aboutUs.ImageFile3, aboutUs.ImageName3);
            }

            if (uploadedfilePath3.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath3[0]);
                aboutUs.ImagePath3 = Convert.ToBase64String(imageArray);
                aboutUs.ImageName3 = uploadedfilePath3[1];
            }
            #endregion

            var res = await _aboutUsRepo.Create(StaticDetail.StaticDetails.createAboutUs, aboutUs, token);

            return RedirectToAction("AddAboutUs");
        }

        [HttpGet("updateAboutUs")]
        public async Task<IActionResult> UpdateAboutUs()
        {
            var aboutus = await _aboutUsRepo.Get(StaticDetail.StaticDetails.getAboutUs);
            return View(aboutus);
        }

        [HttpPost("updateAboutUsContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAboutUsContent([FromForm] AboutUs aboutUs)
        {
            string token = GetToken();
            #region ImageUpload

            aboutUs.ImageName1 = StringProcess.ClearString("content1" + aboutUs.Title) + ".webp";
            var orjaoutusdetails = await _aboutUsRepo.Get(StaticDetail.StaticDetails.getAboutUs);

            if (aboutUs.ImageFile1 == null)
            {
                if (aboutUs.ImageName1 == orjaoutusdetails.ImagePath1)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjaoutusdetails.ImagePath1;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    aboutUs.ImagePath1 = Convert.ToBase64String(imageArray);
                }
                else
                {
                    if (orjaoutusdetails.ImagePath1 != null)
                    {
                        var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjaoutusdetails.ImagePath1;
                        byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                        System.IO.File.Delete(orjpath);
                        var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + aboutUs.ImageName1;
                        System.IO.File.WriteAllBytes(newpath, imageArray);
                        aboutUs.ImagePath1 = Convert.ToBase64String(imageArray);
                    }

                }
            }
            else
            {
                string[] uploadedfilePath = await _fileUpload.UploadFile(aboutUs.ImageFile1, StringProcess.ClearString(aboutUs.Content1));
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    aboutUs.ImagePath1 = Convert.ToBase64String(imageArray);
                }
                else
                {
                    aboutUs.ImageName1 = string.Empty;
                }
            }


            aboutUs.ImageName2 = StringProcess.ClearString("content2" + aboutUs.Title) + ".webp";
            if (aboutUs.ImageFile2 == null)
            {
                if (aboutUs.ImageName2 == orjaoutusdetails.ImagePath2)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjaoutusdetails.ImagePath2;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    aboutUs.ImagePath2 = Convert.ToBase64String(imageArray);
                }
                else
                {
                    if (orjaoutusdetails.ImagePath2 != null)
                    {
                        var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjaoutusdetails.ImagePath2;
                        byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                        System.IO.File.Delete(orjpath);
                        var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + aboutUs.ImageName2;
                        System.IO.File.WriteAllBytes(newpath, imageArray);
                        aboutUs.ImagePath2 = Convert.ToBase64String(imageArray);
                    }

                }
            }
            else
            {
                string[] uploadedfilePath = await _fileUpload.UploadFile(aboutUs.ImageFile2, StringProcess.ClearString(aboutUs.Content2));
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    aboutUs.ImagePath2 = Convert.ToBase64String(imageArray);
                }
                else
                {
                    aboutUs.ImageName2 = string.Empty;
                }
            }


            aboutUs.ImageName3 = StringProcess.ClearString("content3" + aboutUs.Title) + ".webp";
            if (aboutUs.ImageFile3 == null)
            {
                if (aboutUs.ImageName3 == orjaoutusdetails.ImagePath3)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjaoutusdetails.ImagePath3;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    aboutUs.ImagePath3 = Convert.ToBase64String(imageArray);
                }
                else
                {
                    if (orjaoutusdetails.ImagePath3 != null)
                    {
                        var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjaoutusdetails.ImagePath3;
                        byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                        System.IO.File.Delete(orjpath);
                        var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + aboutUs.ImageName3;
                        System.IO.File.WriteAllBytes(newpath, imageArray);
                        aboutUs.ImagePath3 = Convert.ToBase64String(imageArray);
                    }

                }
            }
            else
            {
                string[] uploadedfilePath = await _fileUpload.UploadFile(aboutUs.ImageFile3, StringProcess.ClearString(aboutUs.Content3));
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    aboutUs.ImagePath3 = Convert.ToBase64String(imageArray);
                }
                else
                {
                    aboutUs.ImageName3 = string.Empty;
                }
            }

            #endregion

            var res = await _aboutUsRepo.Update(StaticDetail.StaticDetails.updateAboutUs, aboutUs, token);
            return RedirectToAction("AddAboutUs");
        }


        [Route("deleteAboutUs/{Id}")]
        public async Task<IActionResult> DeleteAboutUs(int Id)
        {
            var aboutUsData = await _aboutUsRepo.Get(StaticDetail.StaticDetails.getAboutUs);
            if (!string.IsNullOrEmpty(aboutUsData.ImagePath1))
            {
                var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + aboutUsData.ImagePath1;
                System.IO.File.Delete(orjpath);
            }
            if (!string.IsNullOrEmpty(aboutUsData.ImagePath2))
            {
                var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + aboutUsData.ImagePath2;
                System.IO.File.Delete(orjpath);
            }
            if (!string.IsNullOrEmpty(aboutUsData.ImagePath3))
            {
                var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + aboutUsData.ImagePath3;
                System.IO.File.Delete(orjpath);
            }

            string token = GetToken();
            bool result = await _aboutUsRepo.Delete(StaticDetail.StaticDetails.deleteCategory + Id, token);
            if (result)
            {
                TempData["success"] = "Hakkımızda silindi.  ";
            }
            else
            {
                TempData["fail"] = "Hakkımızda silinirken bir hata oluştu";
            }

            return RedirectToAction("AddAboutUs");
        }
    }
}
