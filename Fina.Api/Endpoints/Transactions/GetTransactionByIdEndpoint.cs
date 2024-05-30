using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: Get by id")
            .WithSummary("Retorna uma transação")
            .WithDescription("Retorna uma transação")
            .WithOrder(4)
            .Produces<Transaction>(200);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ITransactionHandler handler, [FromBody] GetTransactionByIdRequest request)
    {
        var response = await handler.GetByIdAsync(request);
        return TypedResults.Ok(response);
    }
}