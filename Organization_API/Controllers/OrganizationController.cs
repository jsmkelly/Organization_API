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
                string strSQL = string.Empty;
                DataTable dataTable = new DataTable();
                List<Organization> organizations = new List<Organization>();
                var parameters = Request.Query;
                bool includeAddress = false;
                bool includeMembers = false;
                bool includeTypes = false;
                bool includeSubOrganizations = false;
                int fetch = 50;
                int offset = 0;

                //if parameters are provided, validate them here
                foreach (var param in parameters)
                {
                    switch (param.Key.ToLower().Trim())
                    {
                        case "include_address":
                        case "include_members":
                        case "include_types":
                        case "include_sub_organzations":
                            if (param.Value.ToString()!.Trim().ToLower() == "t" |
                                param.Value.ToString()!.Trim().ToLower() == "f")
                            {
                                if (param.Key.ToLower().Trim() == "include_address" &
                                   param.Value.ToString()!.Trim().ToLower() == "t")
                                {
                                    includeAddress = true;
                                }
                                else if (param.Key.ToLower().Trim() == "include_members" &
                                         param.Value.ToString()!.Trim().ToLower() == "t")
                                {
                                    includeMembers = true;
                                }
                                else if (param.Key.ToLower().Trim() == "include_types" &
                                         param.Value.ToString()!.Trim().ToLower() == "t")
                                {
                                    includeTypes = true;
                                }
                                else if (param.Key.ToLower().Trim() == "include_sub_organzations" &
                                         param.Value.ToString()!.Trim().ToLower() == "t")
                                {
                                    includeSubOrganizations = true;
                                }
                            }
                            else
                            {
                                body = $"Parameter '{param.Key.Trim()}' can only be 'T' for 'True' or 'F' for 'False'." +
                                       $"You have '{param.Value.ToString()!.Trim()}'";
                                result = BadRequest(body);
                                return result;
                            }
                            break;
                        case "fetch":
                            if (int.TryParse(param.Value.ToString()!.Trim().ToLower(), out fetch))
                            {
                                if(fetch > 50 | fetch < 0)
                                {
                                    body = $"Parameter 'fetch' cannot be greater than 50, and cannot be negative. " +
                                           $"You have '{param.Value.ToString()!.Trim()}'";
                                    result = BadRequest(body);
                                    return result;
                                }
                            }
                            else
                            {
                                body = $"Parameter 'fetch' must be an integer value. " +
                                       $"You have '{param.Value.ToString()!.Trim()}'";
                                result = BadRequest(body);
                                return result;
                            }
                            break;
                        case "offset":
                            if(int.TryParse(param.Value.ToString()!.Trim().ToLower(), out offset))
                            {
                                if(offset < 0)
                                {
                                    body = $"Parameter 'offset' cannot be less than 0. " +
                                           $"You have '{param.Value.ToString()!.Trim()}'";
                                    result = BadRequest(body);
                                    return result;
                                }
                            }
                            else
                            {
                                body = $"Parameter 'offset' must be an integer value. " +
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

                //get the data
                strSQL = $@"SELECT * FROM Organizations
                            ORDER BY NAME ASC
                            OFFSET {offset} ROWS 
                            FETCH FIRST {fetch} ROWS ONLY";
                dataTable = GetData(strSQL);

                foreach (DataRow row in dataTable.Rows)
                {
                    Organization org = new Organization(
                        row["Name"].ToString()!.Trim(),
                        row["OrganizationId"].ToString()!.Trim());

                    if (includeAddress)
                    {
                        DataTable locTblAddress = new DataTable();
                        //get the address for the organization
                        //I am only supporting one address per organization for this example
                        locTblAddress = GetData(@$"SELECT A.Name, A.OrganizationId, B.Street, 
                                                   B.City, B.StateProvCode, B.PostalCode, B.Country 
                                                   FROM dbo.Organizations as A 
                                                   LEFT JOIN dbo.Addresses as B ON A.OrganizationId = B.OrganizationId
                                                   WHERE A.OrganizationId = '{row["OrganizationId"].ToString()!.Trim()}'");

                        if (locTblAddress.Rows.Count > 0)
                        {
                            foreach (DataRow addrRow in locTblAddress.Rows)
                            {
                                Address address = new Address(
                                    addrRow["Street"].ToString()!.Trim(),
                                    addrRow["City"].ToString()!.Trim(),
                                    addrRow["StateProvCode"].ToString()!.Trim(),
                                    addrRow["PostalCode"].ToString()!.Trim(),
                                    addrRow["Country"].ToString()!.Trim());

                                org.Addresses.Add(address);
                            }
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
            throw new NotImplementedException();
        }

        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<OrganizationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
