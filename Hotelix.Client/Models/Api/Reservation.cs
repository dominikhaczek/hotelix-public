using System;
using System.ComponentModel.DataAnnotations;

namespace Hotelix.Client.Models.Api
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Imię")]
        public string ClientName { get; set; }
        [Display(Name = "Nazwisko")]
        public string ClientSurname { get; set; }
        public int RoomId { get; set; }
        [Display(Name = "Zameldowanie")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Wymeldowanie")]
        public DateTime EndTime { get; set; }
        public int LocationId { get; set; }
        [Display(Name = "Lokalizacja")]
        public string LocationName { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        /**
         * In reservation management temporary means "total cost"
         */
        [Display(Name = "Cena za dobę")]
        public decimal PricePerNight { get; set; }
        [Display(Name = "Maksymalna liczba gości")]
        public int GuestLimit { get; set; }
        [Display(Name = "Typ łóżka")]
        public string BedType { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "URL obrazka")]
        public string ImageUrl { get; set; }
    }
}