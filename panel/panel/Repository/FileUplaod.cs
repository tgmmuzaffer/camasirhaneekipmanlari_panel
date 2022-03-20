using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public async Task<string[]> UploadFile(IFormFile file, string imageName, bool isblog=default, bool isslider= default)
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
                        //if (extension == ".svg")
                        //{
                        //    imageName += ".svg";
                        //    string path = await SaveImage(file, imageName);
                        //    if (!string.IsNullOrEmpty(path))
                        //    {
                        //        return new[] { path, imageName };
                        //    }

                        //    return Array.Empty<string>();
                        //}
                        if (!string.IsNullOrEmpty(imageName) && !isblog && !isslider)
                        {
                            Size size = new Size(800, 800);
                            imageName += ".webp";
                            string path = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + imageName;
                            using (var webPFileStream = new FileStream(path, FileMode.Create))
                            {
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                imageFactory.Load(file.OpenReadStream())
                                            .Format(new WebPFormat())
                                            .Quality(80)
                                            .Resize(size)
                                            .Save(webPFileStream);
                            }
                            return new[] { path, imageName };
                        }
                        else if (!string.IsNullOrEmpty(imageName) && !isslider && isblog)
                        {
                            Size size = new Size(1200, 500);
                            imageName += ".webp";
                            string path = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + imageName;
                            using (var webPFileStream = new FileStream(path, FileMode.Create))
                            {
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                imageFactory.Load(file.OpenReadStream())
                                            .Format(new WebPFormat())
                                            .Quality(80)
                                            .Resize(size)
                                            .Save(webPFileStream);
                            }
                            return new[] { path, imageName };
                        }
                        else if (!string.IsNullOrEmpty(imageName) && !isblog && isslider)
                        {
                            Size size = new Size(1920, 670);
                            imageName += ".webp";
                            string path = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + imageName;
                            using (var webPFileStream = new FileStream(path, FileMode.Create))
                            {
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                imageFactory.Load(file.OpenReadStream())
                                            .Format(new WebPFormat())
                                            .Quality(80)
                                            .Resize(size)
                                            .Save(webPFileStream);
                            }
                            return new[] { path, imageName };
                        }
                        else
                        {
                            return Array.Empty<string>();
                        }
                    }
                }

                return Array.Empty<string>();
            }
            catch (Exception e)
            {
                return Array.Empty<string>();
            }
        }


        public async Task<string> SaveImage(IFormFile file, string imageName)
        {
            try
            {
                string extension = Path.GetExtension(file.FileName);
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "\\images\\webpImages\\"))
                {
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\images\\webpImages\\");
                }

                string filepath = _hostingEnvironment.WebRootPath + "\\images\\webpImages\\" + imageName;
                using (FileStream filestream = File.Create(filepath))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                }

                return filepath;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
