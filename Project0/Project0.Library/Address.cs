
namespace Project0.Library
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }


        Address(string str, string cit, string sta, int zip)
        {
            Street = str;
            City = cit;
            State = sta;
            Zipcode = zip;
        }
    }
}
