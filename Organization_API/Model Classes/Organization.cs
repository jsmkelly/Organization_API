using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
namespace Organization_API
{
    public class Organization
    {
        private string _name = "";
        private string _id = "";
        private List<Organization> _subOrganizations = null!;
        private List<OrganizationType> _type = null!;
        private Address _address = null!;
        private List<Member> _members = null!;

        public Organization(string name, string id)
        {
            _name = name;
            _id = id;
        }

        public string Name { get => _name; set => _name = value; }
        public string OrganizationId { get => _id; set => _id = value; }
        public List<Organization> SubOrganizations { get => _subOrganizations; set => _subOrganizations = value; }
        public List<OrganizationType> Type { get => _type; set => _type = value; }
        public Address Address { get => _address; set => _address = value; }
        public List<Member> Members { get => _members; set => _members = value; }
    }

}
