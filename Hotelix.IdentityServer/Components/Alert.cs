using Hotelix.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.IdentityServer.Components
{
    public class Alert : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(GetAlertCookie());
        }
        
        public AlertModel GetAlertCookie()
        {
            var alert = new AlertModel
            {
                Type = HttpContext.Request.Cookies["alertType"] ?? "",
                Message = HttpContext.Request.Cookies["alertMessage"] ?? ""
            };
            
            HttpContext.Response.Cookies.Append("alertType", "");
            HttpContext.Response.Cookies.Append("alertMessage", "");

            return alert;
        }
    }
}
