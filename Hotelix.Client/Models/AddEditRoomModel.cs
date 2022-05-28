using System.ComponentModel.DataAnnotations;
using Hotelix.Client.Models.Api;
using Microsoft.AspNetCore.Http;

namespace Hotelix.Client.Models
{
    public class AddEditRoomModel
    {
        public Room Room { get; set; }

        [Display(Name = "Zdjęcie")]
        public IFormFile Image { get; set; }
    }
}
