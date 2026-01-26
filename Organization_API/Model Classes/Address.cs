namespace Organization_API
{
    public class Address
    {
        private string _street = "";
        private string _city = "";
        private string _state = "";
        private string _postalCode = "";
        private string _country = "";

        public Address(string street, string city, string state, string postalCode, string country)
        {
            _street = street;
            _city = city;
            _state = state;
            _postalCode = postalCode;
            _country = country;
        }

        public string Street { get => _street; set => _street = value; }
        public string City { get => _city; set => _city = value; }
        public string State { get => _state; set => _state = value; }
        public string PostalCode { get => _postalCode; set => _postalCode = value; }
        public string Country { get => _country; set => _country = value; }
    }
}
