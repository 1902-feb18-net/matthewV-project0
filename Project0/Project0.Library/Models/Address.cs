
using System;
using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Address
    {
        private string _street;
        private string _country;

        [DataMember]
        public string Street
        {
            get => _street;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Street location must not be null.");
                }

                if (value.Length == 0)
                {
                    throw new ArgumentException("Street location must not be empty.", nameof(value));
                }

                _street = value;
            }
        }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zipcode { get; set; }
        [DataMember]
        public string Country
        {
            get => _country;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Country location must not be null.");
                }

                if (value.Length == 0)
                {
                    throw new ArgumentException("Country location must not be empty.", nameof(value));
                }

                _country = value;
            }

        }

        public override string ToString()
        {
            return Street + ", " + City + ", " + State + ", "  + Zipcode + ", "  + Country;
        }

        public Address()
        {
            Street = "Default Street";
            Country = "US";
        }

        public Address(string str, string cit, string sta, string zip, string country)
        {
            Street = str;
            City = cit;
            State = sta;
            Zipcode = zip;
            Country = country;
        }
    }
}
