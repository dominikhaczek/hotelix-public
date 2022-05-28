using Hotelix.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Client.Utilities
{
    public class AlertHandler
    {
        /*public static void SetAlertCookie(AlertModel alert)
        {
            HttpContext.Response.Cookies.Append("alertType", alert.Type);
            HttpContext.Response.Cookies.Append("alertMessage", alert.Message);
        }
        
        public RedirectToActionResult SetAlertCookieAndRedirect(AlertModel alert, string actionName,
            string controllerName = null, object routeValues = null, string fragment = null)
        {
            SetAlertCookie(alert);
            return RedirectToAction(actionName, controllerName, routeValues, fragment);
        }*/
    }
}