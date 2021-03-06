using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hotelix.IdentityServer.Models;
using Hotelix.IdentityServer.Models.Api;
using Hotelix.IdentityServer.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Hotelix.IdentityServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;

        private readonly IReservationsService _reservationsService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            //IEmailSender emailSender,
            IReservationsService reservationsService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            _reservationsService = reservationsService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane!")]
            [EmailAddress]
            [Display(Name = "Adres e-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole 'Hasło' jest wymagane!")]
            [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i najwyżej {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Pole hasło i potwierdź hasło nie zgadzają się.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Pole 'Numer telefonu' jest wymagane!")]
            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            // Client fields

            [Required(ErrorMessage = "Pole 'Imię' jest wymagane!")]
            [StringLength(50, ErrorMessage = "{0} musi mieć co najmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Imię")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Pole 'Nazwisko' jest wymagane!")]
            [StringLength(50, ErrorMessage = "{0} musi mieć co najmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Pole 'Adres' jest wymagane!")]
            [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Adres")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Pole 'Miasto' jest wymagane!")]
            [StringLength(50, ErrorMessage = "{0} musi mieć co najmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Required(ErrorMessage = "Pole 'Kod pocztowy' jest wymagane!")]
            [StringLength(6, ErrorMessage = "{0} musi mieć {1} znaków.", MinimumLength = 6)]
            [Display(Name = "Kod pocztowy")]
            public string PostalCode { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.PhoneNumber };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    try
                    {
                        var createdClientUserId = await _reservationsService.CreateClient(new Client()
                        {
                            UserId = user.Id,
                            Name = Input.Name,
                            Surname = Input.Surname,
                            Address = Input.Address,
                            City = Input.City,
                            PostalCode = Input.PostalCode
                        });

                        if (string.IsNullOrEmpty(createdClientUserId))
                        {
                            throw new ApplicationException();
                        }
                    }
                    catch (Exception)
                    {
                        return Page();
                    }

                    await _userManager.AddToRoleAsync(user, "Klient");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
