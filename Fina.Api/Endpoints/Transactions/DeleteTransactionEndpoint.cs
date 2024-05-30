using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Exclui uma transação")
            .WithDescription("Exclui uma transação")
            .WithOrder(3)
            .Produces(204);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ITransactionHandler handler, [FromBody] DeleteTransactionRequest request)
    {
        var response = await handler.DeleteAsync(request);
        return response.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.BadRequest(response);
    }
}