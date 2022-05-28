using System.ComponentModel.DataAnnotations;

namespace Hotelix.Client.Models.Api
{
    public class Client
    {
        public string UserId { get; set; }
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
    }
}