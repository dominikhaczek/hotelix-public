using System.ComponentModel.DataAnnotations;

namespace Hotelix.Client.Models.Api
{
    public class ReservationsLocation
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
    }
}