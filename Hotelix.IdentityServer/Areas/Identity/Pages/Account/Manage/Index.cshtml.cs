using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.IdentityServer.Exceptions;
using Hotelix.IdentityServer.Models;
using Hotelix.IdentityServer.Models.Api;
using Hotelix.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using Hotelix.Client.Models.Database;

namespace Hotelix.IdentityServer.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IReservationsService _reservationsService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IReservationsService reservationsService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _reservationsService = reservationsService;
        }

        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            // Client fields
            [Required]
            [StringLength(50, ErrorMessage = "{0} musi mieć conajmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Imię")]
            public string Name { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "{0} musi mieć conajmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi mieć conajmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Adres")]
            public string Address { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "{0} musi mieć conajmniej {2} i najwyżej {1} znaków.", MinimumLength = 2)]
            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Required]
            [StringLength(6, ErrorMessage = "{0} musi mieć {1} znaków.", MinimumLength = 6)]
            [Display(Name = "Kod pocztowy")]
            public string PostalCode { get; set; }

            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            Client client = null;

            try
            {
                client = await _reservationsService.GetClient(user.Id);
            }
            catch (NotFoundException)
            {
            }

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = client?.Name,
                Surname = client?.Surname,
                Address = client?.Address,
                City = client?.City,
                PostalCode = client?.PostalCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie udało się załadować użytkownika z identyfikatorem '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var isClient = await _userManager.IsInRoleAsync(user, "Klient");

            if (user == null)
            {
                return NotFound($"Nie udało się załadować użytkownika z identyfikatorem '{_userManager.GetUserId(User)}'.");
            }

            if (!isClient)
            {
                ModelState.Remove("Input.Name");
                ModelState.Remove("Input.Surname");
                ModelState.Remove("Input.Address");
                ModelState.Remove("Input.City");
                ModelState.Remove("Input.PostalCode");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            
            if (isClient)
            {
                var client = await _reservationsService.GetClient(user.Id);
                var inputClient = new Client()
                {
                    UserId = client.UserId,
                    Name = Input.Name,
                    Surname = Input.Surname,
                    Address = Input.Address,
                    City = Input.City,
                    PostalCode = Input.PostalCode
                };
                if (!client.Equals(inputClient))
                {
                    try
                    {
                        await _reservationsService.UpdateClient(inputClient);
                    }
                    catch (Exception)
                    {
                        await LoadAsync(user);
                        return Page();
                    }
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twoje dane osobowe zostały zaktualizowane!";
            return RedirectToPage();
        }
    }
}
