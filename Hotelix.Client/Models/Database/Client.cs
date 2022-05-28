using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hotelix.Client.Models.Database
{
    public partial class Client
    {
        /*public Client()
        {
            Reservations = new HashSet<Reservation>();
        }*/

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public virtual AspNetUser User { get; set; }
        //public virtual ICollection<Reservation> Reservations { get; set; }

        public bool IsEqual(Client client)
        {
            return this.Id == client.Id &&
                   this.UserId == client.UserId &&
                   this.Name == client.Name &&
                   this.Surname == client.Surname &&
                   this.Address == client.Address &&
                   this.City == client.City &&
                   this.PostalCode == client.PostalCode;
        }
    }
}
