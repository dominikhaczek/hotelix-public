using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Client.Models;
using Hotelix.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Client.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly IOfferService _offerService;
        public CategoryMenu(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var locations = (await _offerService.GetAllLocations()).OrderBy(c => c.Name);
            return View(locations);
        }
    }
}
