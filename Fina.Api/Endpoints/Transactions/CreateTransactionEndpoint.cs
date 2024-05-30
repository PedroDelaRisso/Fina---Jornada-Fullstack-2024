using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Cria uma nova transação")
            .WithDescription("Cria uma nova transação")
            .WithOrder(1)
            .Produces(201);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ITransactionHandler handler, [FromBody] CreateTransactionRequest request)
    {
        var response = await handler.CreateAsync(request);
        return response.IsSuccess
            ? TypedResults.Created($"/transactions/{response.Data?.Id}", response)
            : TypedResults.BadRequest(response);
    }
}