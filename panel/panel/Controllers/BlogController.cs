using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using panel.Models;
using panel.Models.Dtos;
using panel.Extensions;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{

    public class BlogController : Controller
    {
        private readonly IBlogRepo _blogRepo;
        private readonly ITagRepo _tagRepo;
        private readonly IFileUpload _fileUpload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BlogController(IBlogRepo blogRepo, IFileUpload fileUpload, ITagRepo tagRepo, IHostingEnvironment hostingEnvironment)
        {
            _blogRepo = blogRepo;
            _fileUpload = fileUpload;
            _tagRepo = tagRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route(template: "addBlog", Name = "Blog Ekle")]
        public async Task<IActionResult> AddBlog()
        {

            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var result = await _tagRepo.GetList(StaticDetail.StaticDetails.getAllTags, token);
            List<SelectListItem> tagsList = new List<SelectListItem>();
            foreach (var item in result)
            {
                tagsList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false) });
            }
            ViewBag.tags = tagsList;
            return View();
        }

        [HttpPost("createblog")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlog([FromForm] BlogDto blog)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            blog.ImagePath = StringProcess.ClearString(blog.Title);
            blog.ImageName = blog.ImagePath;
            string uploadedfilePath = await _fileUpload.UploadFile(blog.ImageFile, blog.ImagePath+".webp");
            if (!string.IsNullOrEmpty(uploadedfilePath))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                blog.ImagePath = Convert.ToBase64String(imageArray);
            }
            var res = await _blogRepo.Create(StaticDetail.StaticDetails.createBlog, blog, token);

            return RedirectToAction("GetAllBlogs");
        }

        [Route(template: "getAllBlogs", Name = "Blog Listesi")]
        public async Task<IActionResult> GetAllBlogs()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var blogs = await _blogRepo.GetList(StaticDetail.StaticDetails.getAllBlogs, token);
            return View(blogs);
        }

        [HttpGet("updateBlog/{Id}")]
        public async Task<IActionResult> UpdateBlog(int Id)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            var blog = await _blogRepo.Get(StaticDetail.StaticDetails.getBlog + Id, token);
            var tags = await _tagRepo.GetList(StaticDetail.StaticDetails.getAllTags, token);
           
            List<SelectListItem> tagsList = new List<SelectListItem>();
            foreach (var item in tags)
            {
                tagsList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (blog.TagIds.Contains(item.Id) ? true : false)
                });
            }
            ViewBag.tags = tagsList;
            return View(blog);
        }
        [HttpPost("updateBlogContent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBlogContent([FromForm] BlogDto blog)
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            string uploadedfilePath = string.Empty;
            if (value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }

            blog.ImageName = StringProcess.ClearString(blog.Title);
            if (blog.ImageFile == null)
            {
                var orjblogdetails = await _blogRepo.Get(StaticDetail.StaticDetails.getBlog + blog.Id, token);
                if ((blog.ImageName+".webp" )== orjblogdetails.ImagePath)
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjblogdetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    blog.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + orjblogdetails.ImagePath;
                    byte[] imageArray = System.IO.File.ReadAllBytes(orjpath);
                    System.IO.File.Delete(orjpath);
                    var newpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + blog.ImageName + ".webp";
                    System.IO.File.WriteAllBytes(newpath, imageArray);
                    blog.ImagePath = Convert.ToBase64String(imageArray);
                }
            }
            else
            {
                uploadedfilePath = await _fileUpload.UploadFile(blog.ImageFile, blog.ImageName);
                if (!string.IsNullOrEmpty(uploadedfilePath))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(uploadedfilePath);
                    blog.ImagePath = Convert.ToBase64String(imageArray);
                }
                else
                {
                    blog.ImageName = string.Empty;
                }
            }

            var res = await _blogRepo.Update(StaticDetail.StaticDetails.updateBlog, blog, token);
            return RedirectToAction("GetAllBlogs");
        }

        [Route("deleteBlog/{id}/{title}")]
        public async Task<IActionResult> DeleteBlog(int Id, string title)
        {
            HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value != null && value.Length > 0)
            {
                token = Encoding.Default.GetString(value);
            }
            string imgPath = StringProcess.ClearString(title);
            bool result = await _blogRepo.Delete(StaticDetail.StaticDetails.deleteBlog + Id, token);
            var orjpath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + imgPath + ".webp";
            System.IO.File.Delete(orjpath);
            if (result)
            {
                TempData["success"] = "Blog silindi.  ";
            }
            else
            {
                TempData["fail"] = "Blog silinirken bir hata oluştu";
            }

            return RedirectToAction("GetAllBlogs");
        }
    }
}
