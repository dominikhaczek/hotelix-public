using System;
using Hotelix.Client.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotelix.Client.Models.Api;

namespace Hotelix.Client.ViewModels
{
    public class RoomListViewModel : IValidatableObject
    {
        public IEnumerable<Room> Rooms { get; set; }

        [Display(Name = "Lokalizacja")]
        public int? CurrentLocation { get; set; }

        [Display(Name = "Data od")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Display(Name = "Data do")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (StartTime > EndTime)
            {
                errors.Add(new ValidationResult($"'{nameof(EndTime)}' musi następować po 'Data od'.", new List<string> { nameof(EndTime) }));
            }
            return errors;
        }
    }
}
