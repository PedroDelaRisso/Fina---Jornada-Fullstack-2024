using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", HandleAsync)
            .WithName("Categories: Get all")
            .WithSummary("Retorna todas as categorias")
            .WithDescription("Retorna todas as categorias")
            .WithOrder(5)
            .Produces<PagedResponse<List<Category?>>>(200);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ICategoryHandler handler, [FromBody] GetAllCategoriesRequest request)
    {
        var response = await handler.GetAllAsync(request);
        return TypedResults.Ok(response);
    }
}