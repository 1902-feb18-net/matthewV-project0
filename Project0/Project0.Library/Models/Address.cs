
using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zipcode { get; set; }
        [DataMember]
        public string Country { get; set; }

        public override string ToString()
        {
            return Street + ", " + City + ", " + State + ", "  + Zipcode + ", "  + Country;
        }

        public Address()
        {
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
