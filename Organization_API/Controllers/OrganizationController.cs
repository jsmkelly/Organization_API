using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Organization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : BaseController
    {

        // GET: api/<OrganizationController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                DataTable dataTable = GetData("SELECT * FROM dbo.Organizations");
                List<Organization> organizations = new List<Organization>();
                var parameters = Request.Query;
                bool includeAddress = false;

                //if parameters are provided, validate them here
                foreach (var param in parameters)
                {
                    switch (param.Key.ToLower().Trim())
                    {
                        case "include_address":
                            if (param.Value.ToString()!.Trim().ToLower() == "t" |
                                param.Value.ToString()!.Trim().ToLower() == "f")
                            {
                                includeAddress = param.Value.ToString()!.ToLower() == "t";
                            }
                            else
                            {
                                body = $"Parameter '{param.Key.Trim()}' can only be 'T' for 'True' or 'F' for 'False'." +
                                       $"You have '{param.Value.ToString()!.Trim()}'";
                                result = BadRequest(body);
                                return result;
                            }
                            break;
                        default:
                            body = $"Parameter '{param.Key.Trim()}' is not supported.";
                            result = BadRequest(body);
                            return result;
                    }
                }

                foreach (DataRow row in dataTable.Rows)
                {
                        Organization org = new Organization(
                            row["Name"].ToString()!.Trim(),
                            row["OrganizationId"].ToString()!.Trim());
                        if (includeAddress)
                        {
                            DataTable address = new DataTable();
                            //get the address for the organization
                            //I am only supporting one address per organization for this example
                            address = GetData(@$"SELECT TOP 1 A.Name, A.OrganizationId, B.Street, 
                                                 B.City, B.StateProvCode, B.PostalCode, B.Country 
                                                 FROM dbo.Organizations as A 
                                                 LEFT JOIN dbo.Addresses as B ON A.OrganizationId = B.OrganizationId
                                                 WHERE A.OrganizationId = '{row["OrganizationId"].ToString()!.Trim()}'");

                            if (address.Rows.Count == 1)
                            {
                                org.Address = new Address(
                                    address.Rows[0]["Street"].ToString()!.Trim(),
                                    address.Rows[0]["City"].ToString()!.Trim(),
                                    address.Rows[0]["StateProvCode"].ToString()!.Trim(),
                                    address.Rows[0]["PostalCode"].ToString()!.Trim(),
                                    address.Rows[0]["Country"].ToString()!.Trim());
                            }
                            else
                            {

                            }
                        }
                        organizations.Add(org);
                }

                body = JsonConvert.SerializeObject(organizations, 
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore, 
                        Formatting = Formatting.Indented
                    });

                result = Ok(body);

                return result;
            }
            catch (Exception ex)
            {
                body = ex.Message;
                result = StatusCode(StatusCodes.Status500InternalServerError, body);
                return result;
            }
        }

        // GET api/<OrganizationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrganizationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrganizationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
