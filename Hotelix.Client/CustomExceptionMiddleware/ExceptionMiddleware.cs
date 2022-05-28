using System;
using System.Threading.Tasks;
using Hotelix.Client.Models;
using Microsoft.AspNetCore.Http;

namespace Hotelix.Client.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception)
            {
                SetAlertCookieAndRedirect(httpContext, new AlertModel
                {
                    Type = "danger",
                    Message = "Ups, coś poszło nie tak. Spróbuj ponownie za chwilę."
                });
            }
        }
        
        private static void SetAlertCookie(HttpContext context, AlertModel alert)
        {
            context.Response.Cookies.Append("alertType", alert.Type);
            context.Response.Cookies.Append("alertMessage", alert.Message);
        }
        
        private static void SetAlertCookieAndRedirect(HttpContext context, AlertModel alert, string location = "/")
        {
            SetAlertCookie(context, alert);
            context.Response.Redirect(location);
        }
    }
}