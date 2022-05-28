namespace Hotelix.Reservations.Models.Api
{
    public class LocationApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public bool IsHidden { get; set; }
    }
}