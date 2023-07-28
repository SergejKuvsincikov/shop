using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAgregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstName, string lastmame, string city, string street, string state, string zipCode)
        {
            FirstName = firstName;
            Lastmame = lastmame;
            City = city;
            Street = street;
            State = state;
            ZipCode = zipCode;
        }

        public string FirstName { get; set; }
        public string Lastmame { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}