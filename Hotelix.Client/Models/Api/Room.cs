using System;
using System.ComponentModel.DataAnnotations;

namespace Hotelix.Client.Models.Api
{
    public class Room
    {
        public int Id { get; set; }

        [Display(Name = "Lokalizacja")]
        public int LocationId { get; set; }
        
        [Display(Name = "Lokalizacja")]
        public string Location { get; set; }
        
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        
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
        
        [Display(Name = "Start aktywności")]
        public DateTime? StartTime { get; set; }
        
        [Display(Name = "Koniec aktywności")]
        public DateTime? EndTime { get; set; }

        public bool IsHidden { get; set; }
    }
}
