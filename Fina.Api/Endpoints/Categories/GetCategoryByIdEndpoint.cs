using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: Get by id")
            .WithSummary("Retorna uma categoria")
            .WithDescription("Retorna uma categoria")
            .WithOrder(4)
            .Produces<Category>(200);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ICategoryHandler handler, [FromBody] GetCategoryByIdRequest request)
    {
        var response = await handler.GetByIdAsync(request);
        return TypedResults.Ok(response);
    }
}