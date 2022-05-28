using System.Linq;
using System.Threading.Tasks;
using Hotelix.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Hotelix.IdentityServer.Quickstart.UserManagement
{
    [Authorize(Roles = "Administrator")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(UserManager<ApplicationUser> userManager, ILogger<UserManagementController> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var rolesList = _roleManager.Roles.Where(r => r.Name != "Klient").Select(a => new SelectListItem()
                {
                    Value = a.Name,
                    Text = a.Name
                })
                .ToList();

            ViewBag.RolesList = rolesList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateUserViewModel createUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = createUserViewModel.Email, Email = createUserViewModel.Email, PhoneNumber = createUserViewModel.PhoneNumber };
                var result = await _userManager.CreateAsync(user, createUserViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, createUserViewModel.UserRole);
                    _logger.LogInformation("User created a new account with password.");
                    SetAlertCookie(new AlertModel
                    {
                        Type = "success",
                        Message = "Pomyślnie dodano pracownika."
                    });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                SetAlertCookie(new AlertModel
                {
                    Type = "danger",
                    Message = "Pracownik nie został dodany - nie wszystkie wymagane pola zostały poprawnie uzupełnione."
                });
            }
            
            return RedirectToAction("Index");
        }
        
        
        
        public void SetAlertCookie(AlertModel alert)
        {
            HttpContext.Response.Cookies.Append("alertType", alert.Type);
            HttpContext.Response.Cookies.Append("alertMessage", alert.Message);
        }
        
        public RedirectToActionResult SetAlertCookieAndRedirect(AlertModel alert, string actionName,
            string controllerName = null, object routeValues = null, string fragment = null)
        {
            SetAlertCookie(alert);
            return RedirectToAction(actionName, controllerName, routeValues, fragment);
        }
    }
}
