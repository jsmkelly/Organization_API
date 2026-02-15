using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Organization_API.Model_Classes;
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
                BaseModel<Organization> organizations;
                var parameters = Request.Query;
                bool includeAddress = false;
                int fetch = 50;
                int offset = 0;

                //if parameters are provided, validate them here
                foreach (var param in parameters)
                {
                    switch (param.Key.ToLower().Trim())
                    {//optional parameters for this endpoint are include_address, fetch, and offset
                        //you can get the addresses for the organizations
                        //usually I would add addition pagination on the address level, but it's only a demo
                        case "include_address":
                            if (param.Value.ToString()!.Trim().ToLower() == "t" |
                                param.Value.ToString()!.Trim().ToLower() == "f")
                            {
                                if (param.Key.ToLower().Trim() == "include_address" &
                                   param.Value.ToString()!.Trim().ToLower() == "t")
                                {
                                    includeAddress = true;
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

                organizations = new BaseModel<Organization>(offset, fetch);

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
                                                   B.City, B.StateProvCode, B.PostalCode, C.Name as 'Country'
                                                   FROM dbo.Organizations as A 
                                                   LEFT JOIN dbo.Addresses as B ON A.OrganizationId = B.OrganizationId
                                                   LEFT JOIN dbo.Country as C ON B.CountryCode = C.CountryCode
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

                    organizations.Data.Add(org);
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
        public IActionResult Get(string id)
        {
            try
            {
                string strSQL = string.Empty;
                DataTable dataTable = new DataTable();
                BaseModel<Organization> organizations;
                var parameters = Request.Query;
                bool includeAddress = false;
                int fetch = 1;
                int offset = 0;

                //if parameters are provided, validate them here
                foreach (var param in parameters)
                {
                    switch (param.Key.ToLower().Trim())
                    {   //you can get the address for the organizations
                        //usually I would add addition pagination on the address level, but it's only a demo
                        case "include_address":
                            if (param.Value.ToString()!.Trim().ToLower() == "t" |
                                param.Value.ToString()!.Trim().ToLower() == "f")
                            {
                                if (param.Key.ToLower().Trim() == "include_address" &
                                   param.Value.ToString()!.Trim().ToLower() == "t")
                                {
                                    includeAddress = true;
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
                        default:
                            body = $"Parameter '{param.Key.Trim()}' is not supported.";
                            result = BadRequest(body);
                            return result;
                    }
                }

                organizations = new BaseModel<Organization>(offset, fetch);

                //get the data
                strSQL = $@"SELECT * FROM Organizations
                            WHERE ORGANIZATIONID = '{id}'
                            ORDER BY NAME ASC";
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
                                                   B.City, B.StateProvCode, B.PostalCode, C.Name as 'Country'
                                                   FROM dbo.Organizations as A 
                                                   LEFT JOIN dbo.Addresses as B ON A.OrganizationId = B.OrganizationId
                                                   LEFT JOIN dbo.Country as C ON B.CountryCode = C.CountryCode
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

                    organizations.Data.Add(org);
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
