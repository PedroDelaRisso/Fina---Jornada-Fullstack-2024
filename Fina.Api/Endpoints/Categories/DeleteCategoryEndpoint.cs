using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Requests.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", HandleAsync)
            .WithName("Categories: Delete")
            .WithSummary("Exclui uma categoria")
            .WithDescription("Exclui uma categoria")
            .WithOrder(3)
            .Produces(204);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ICategoryHandler handler, [FromBody] DeleteCategoryRequest request)
    {
        var response = await handler.DeleteAsync(request);
        return response.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.BadRequest(response);
    }
}