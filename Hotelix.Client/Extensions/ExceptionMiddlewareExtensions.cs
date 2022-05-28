using Hotelix.Client.CustomExceptionMiddleware;
using Microsoft.AspNetCore.Builder;

namespace Hotelix.Client.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}