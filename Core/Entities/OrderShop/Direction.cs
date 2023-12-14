using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderShop
{
    public class Direction
    {
        public Direction()
        {
            
        }
        public Direction(string street, string city, string department, string postalCode, string country)
        {
            this.Street = street;
            this.City = city;
            this.Department = department;
            this.PostalCode = postalCode;
            this.Country = country;
        }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Department { get; set; }
        public string Country { get; set; }
    }
}
