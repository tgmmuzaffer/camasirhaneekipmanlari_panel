using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class FileUplaod : IFileUpload
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileUplaod(IHttpClientFactory clientFactory, IHostingEnvironment hostingEnvironment)
        {
            _clientFactory = clientFactory;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<string> UploadFile(IFormFile file, string imageName)
        {
            List<string> whitelist = new List<string>()
            {
                ".webp",
                ".jpg",
                ".png",
                ".gif",
                ".tiff",
                ".tif",
                ".psd",
                ".bmp",
                ".jpeg",
                ".jpe",
                ".jps",
                ".docx",
                ".pdf",
                ".xls",
                ".xlsx",
                ".svg"
            };
            try
            {
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (whitelist.Any(a => extension.Contains(a)))
                    {
                        //string path = await SaveImage(file, imageName);
                        if (!string.IsNullOrEmpty(imageName))
                        {
                            string path = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + imageName;
                            using (var webPFileStream = new FileStream(path, FileMode.Create))
                            {
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                imageFactory.Load(file.OpenReadStream())
                                            .Format(new WebPFormat())
                                            .Quality(80)
                                            .Save(webPFileStream);
                            }
                            return path;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                throw new Exception("Dosya Yüklenemedi.______" + e.Message);
            }
        }


        //public async Task<string> SaveImage(IFormFile file, string imageName)
        //{
        //    try
        //    {
        //        string extension = Path.GetExtension(file.FileName);
        //        imageName += extension;
        //        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "\\images\\"))
        //        {
        //            Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\images\\");
        //        }

        //            using (FileStream filestream = File.Create(_hostingEnvironment.WebRootPath + "\\images\\" + imageName))
        //            {
        //                file.CopyTo(filestream);
        //                filestream.Flush();
        //            }
        //            return file.FileName;

        //    }
        //    catch (Exception e)
        //    {
        //        return string.Empty;
        //    }
        //}
    }
}
