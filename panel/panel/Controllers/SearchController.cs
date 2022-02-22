using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using panel.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public SearchController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [Route("searchpage")]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost("search")]
        public IActionResult Search(string searchText)
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Where(ad => ad.AttributeRouteInfo != null && ad.AttributeRouteInfo.Name != null && ad.AttributeRouteInfo.Name.Contains(searchText))
                .Select(ad => new SearchResultDto
                {
                    SearhResultUrl = ad.AttributeRouteInfo.Template,
                    SearhResultUrlName = ad.AttributeRouteInfo.Name
                })
                .ToList();
            SearchDto res = new()
            {
                SearchText = searchText,
                SearchResultDtos = routes
            };
            var jsonres = JsonConvert.SerializeObject(res);
            return Json(jsonres);

        }
    }
}
