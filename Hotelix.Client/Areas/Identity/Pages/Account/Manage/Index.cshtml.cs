using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hotelix.Client.Models.Database;

namespace Hotelix.Client.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IClientRepository _clientRepository;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IClientRepository clientRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientRepository = clientRepository;
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

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var client = await _clientRepository.GetClientByUserIdAsync(user.Id);

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
                var client = await _clientRepository.GetClientByUserIdNoTrackingAsync(user.Id);
                var inputClient = new Models.Database.Client
                {
                    Id = client.Id,
                    UserId = client.UserId,
                    Name = Input.Name,
                    Surname = Input.Surname,
                    Address = Input.Address,
                    City = Input.City,
                    PostalCode = Input.PostalCode
                };
                if (!client.IsEqual(inputClient))
                {
                    await _clientRepository.UpdateClientAsync(inputClient);
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twoje dane osobowe zostały zaktualizowane!";
            return RedirectToPage();
        }
    }
}
