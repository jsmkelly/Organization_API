using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
namespace Organization_API
{
    public class Organization
    {
        private string _name = "";
        private string _id = "";
        private List<Organization> _subOrganizations = new List<Organization>();
        private List<OrganizationType> _type = new List<OrganizationType>();
        private List<Address> _addresses = new List<Address>();
        private List<Member> _members = new List<Member>();

        public Organization(string name, string id)
        {
            _name = name;
            _id = id;
        }

        public string Name { get => _name; set => _name = value; }
        public string OrganizationId { get => _id; set => _id = value; }

        public void AddAddress(Address address)
        {
            //only add if the an address is provided
            if (address.Street.Trim() == "" & address.State.Trim() == "" &
               address.City.Trim() == "" & address.Country.Trim() == "")
            {
                //do not add an empty address
            }
            else
            {
                _addresses.Add(address);
            }
        }

        public List<Address> Addresses
        {
            get
            {
                if (_addresses.Count > 0)
                {
                    return _addresses;
                }
                else
                {
                    return null;
                }
            }
        }
    }

}
