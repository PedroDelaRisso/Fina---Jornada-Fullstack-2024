using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Atualiza uma transação")
            .WithDescription("Atualiza uma transação")
            .WithOrder(2)
            .Produces(204);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ITransactionHandler handler, [FromBody] UpdateTransactionRequest request)
    {
        var response = await handler.UpdateAsync(request);
        return response.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.BadRequest(response);
    }
}