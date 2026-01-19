using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Organization_API
{
    public class Organization
    {
        private string _name = "";
        private string _id = "";
        private List<Organization> _subOrganizations = new List<Organization>();
        private List<OrganizationType> _type = new List<OrganizationType>();
        private Address _address = null!;
        private List<Member> _members = new List<Member>();

        public string Name { get => _name; set => _name = value; }
        public string Id { get => _id; set => _id = value; }
        public List<Organization> SubOrganizations { get => _subOrganizations; set => _subOrganizations = value; }
        public List<OrganizationType> Type { get => _type; set => _type = value; }
        public Address Address { get => _address; set => _address = value; }
        public List<Member> Members { get => _members; set => _members = value; }
    }

}
