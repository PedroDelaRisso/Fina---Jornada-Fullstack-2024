using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/period", HandleAsync)
            .WithName("Transactions: Get by period")
            .WithSummary("Retorna transações por período")
            .WithDescription("Retorna transações por período")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>>>(200);
    }
    
    private static async Task<IResult> HandleAsync([FromServices] ITransactionHandler handler, [FromBody] GetTransactionsByPeriodRequest request)
    {
        var response = await handler.GetByPeriodAsync(request);
        return TypedResults.Ok(response);
    }
}