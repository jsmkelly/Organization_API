using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Organization_API;
using System.Data;
using System.Data.SqlClient;
namespace Organization_API.Controllers;

public static class OrganizationController
{
    public static void MapOrganizationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Organization").WithTags(nameof(Organization));

        group.MapGet("/", () =>
        {
            DataTable dataTable = SQLActions.GetData("SELECT * FROM dbo.Organizations");
            return new [] { new Organization() };
        })
        .WithName("GetAllOrganizations")
        .WithOpenApi();

        group.MapGet("/{id}", (int id) =>
        {
            //return new Organization { ID = id };
        })
        .WithName("GetOrganizationById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Organization input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateOrganization")
        .WithOpenApi();

        group.MapPost("/", (Organization model) =>
        {
            //return TypedResults.Created($"/api/Organizations/{model.ID}", model);
        })
        .WithName("CreateOrganization")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Organization { ID = id });
        })
        .WithName("DeleteOrganization")
        .WithOpenApi();
    }
}
