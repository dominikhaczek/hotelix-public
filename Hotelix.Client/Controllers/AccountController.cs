using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Hotelix.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Register() =>
            Redirect(_configuration["ApiConfigs:IdentityServer:Uri"] + "Identity/Account/Register?returnUrl=" + WebUtility.UrlEncode(_configuration["ApiConfigs:Client:Uri"]));

        public IActionResult ManageAccount() =>
            Redirect(_configuration["ApiConfigs:IdentityServer:Uri"] + "Identity/Account/Manage/Index");

        public IActionResult AddAccount() =>
            Redirect(_configuration["ApiConfigs:IdentityServer:Uri"] + "UserManagement");
    }
}
