using Microsoft.AspNetCore.Mvc;
using panel.Repository.IRepository;
using System.Text;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class PropertyTabController : Controller
    {
        private readonly IPropertyTabRepo _propertyTabRepo;
        public PropertyTabController(IPropertyTabRepo propertyTabRepo)
        {
            _propertyTabRepo = propertyTabRepo;
        }
        public async Task<IActionResult> AllProperties()
        {
            this.HttpContext.Session.TryGetValue("Jwt", out byte[] value);
            string token = string.Empty;
            if (value.Length > 0)
            {
                token= Encoding.Default.GetString(value);
            }
            var resuylt = await _propertyTabRepo.GetList(StaticDetail.StaticDetails.getAllProperties, token);
            return View(resuylt);
        }
    }
}
