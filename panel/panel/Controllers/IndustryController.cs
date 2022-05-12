using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using panel.Extensions;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class IndustryController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IIndustryRepo _industryRepo;
        private readonly IFileUpload _fileUpload;

        public IndustryController(IHostingEnvironment hostingEnvironment, IIndustryRepo industryRepo, IFileUpload fileUpload)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileUpload = fileUpload;
            _industryRepo = industryRepo;
        }

        [Route(template: "addIndustry", Name = "Sektör Ekle")]
        public async Task<IActionResult> AddIndustry()
        {
            return View();
        }

        [HttpPost("createIndustry")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIndustry([FromForm] Industry industry)
        {
            string token = GetToken();
            string imgName = industry.Description[..3];
            industry.ImagePath = StringProcess.ClearString(imgName);

            string[] uploadedfilePath = await _fileUpload.UploadFile(industry.ImageFile, industry.ImagePath, isIndustry: true);
            if (uploadedfilePath.Length != 0)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                industry.ImageData = Convert.ToBase64String(imageArray);
            }
            var res = await _industryRepo.Create(StaticDetail.StaticDetails.createIndustry, industry, token);

            return RedirectToAction("GetAllIndustry");
        }

        [Route(template: "getAllIndustry", Name = "Sektör Listesi")]
        public async Task<IActionResult> GetAllIndustry()
        {
            string token = GetToken();
            var industries = await _industryRepo.GetList(StaticDetail.StaticDetails.getIndustries, token);
            return View(industries);
        }

        [HttpGet("updateIndustry/{Id}")]
        public async Task<IActionResult> UpdateIndustry(int Id)
        {
            string token = GetToken();

            var industry = await _industryRepo.Get(StaticDetail.StaticDetails.getIndustry + Id, token);
           
            return View(industry);
        }

        [HttpPost("updateIndustryContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBlogContent([FromForm] Industry industry)
        {

            string[] uploadedfilePath = Array.Empty<string>();
            string token = GetToken();
            string imgName = industry.Description[..3];
            industry.ImagePath = StringProcess.ClearString(imgName);
            if (industry.ImageFile == null)
            {
                var orjindustrydetails = await _industryRepo.Get(StaticDetail.StaticDetails.getIndustry + industry.Id, token);
                if ((industry.ImagePath + ".webp") == orjindustrydetails.ImagePath)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjindustrydetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    industry.ImageData = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjindustrydetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + industry.ImagePath + ".webp";
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    industry.ImageData = Convert.ToBase64String(imageArray); 
                }
            }
            else
            {
                uploadedfilePath = await _fileUpload.UploadFile(industry.ImageFile, industry.ImagePath, isIndustry: true);
                if (uploadedfilePath.Length != 0)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath[0]);
                    industry.ImageData = Convert.ToBase64String(imageArray);
                }
                else
                {
                    industry.ImageData = string.Empty;
                }
            }

            var res = await _industryRepo.Update(StaticDetail.StaticDetails.updateIndustry, industry, token);
            return RedirectToAction("GetAllIndustry");
        }

        [Route("deleteIndustry/{id}/{name}")]
        public async Task<IActionResult> DeleteIndustry(int Id, string name)
        {
            string token = GetToken();
            bool result = await _industryRepo.Delete(StaticDetail.StaticDetails.deleteIndustry + Id, token);
            var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + name + ".webp";
            System.IO.File.Delete(orjpath);
            if (result)
            {
                TempData["success"] = "Industry silindi.  ";
            }
            else
            {
                TempData["fail"] = "Industry silinirken bir hata oluştu";
            }

            return RedirectToAction("GetAllIndustry");
        }
    }
} 
