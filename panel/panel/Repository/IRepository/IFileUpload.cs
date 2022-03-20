using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IFileUpload
    {
        Task<string[]> UploadFile(IFormFile file, string imageName, bool isblog=default, bool isslider = default);
    }
}
