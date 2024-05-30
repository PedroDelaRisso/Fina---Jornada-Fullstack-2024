using System.Data.Common;
using Fina.Core.Common;
using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var transaction = new Transaction
        {
            UserId = request.UserId,
            Amount = request.Amount,
            Title = request.Title,
            CategoryId = request.CategoryId,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
            PaidOrReceivedAt = request.PaidOrReceveidAt
        };

        try
        {
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
        }
        catch (DbUpdateException)
        {
            return new Response<Transaction?>(null, 500, "Não foi possível criar a transação");
        }

    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var transaction = new Transaction
        {
            Id = request.Id,
            UserId = request.UserId,
            Amount = request.Amount,
            Title = request.Title,
            CategoryId = request.CategoryId,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
            PaidOrReceivedAt = request.PaidOrReceveidAt
        };

        try
        {
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação editada com sucesso");
        }
        catch (DbUpdateException)
        {
            return new Response<Transaction?>(null, 500, "Não foi possível criar a transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id);
            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação excluída com sucesso");
        }
        catch (DbUpdateException)
        {
            return new Response<Transaction?>(null, 500, "Não foi possível excluir a transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);
            return transaction is null
                ? new Response<Transaction?>(null, 404, "Transação não encontrada")
                : new Response<Transaction?>(transaction, 201, "Transação encontrada com sucesso");
        }
        catch (DbException)
        {
            return new Response<Transaction?>(null, 500, "Não foi buscar a transação");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetLastDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
            var query = context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.PaidOrReceivedAt >= request.StartDate &&
                    t.PaidOrReceivedAt <= request.EndDate &&
                    t.UserId == request.UserId)
                .OrderBy(d => d.PaidOrReceivedAt);

            var transactions = await query
                .Skip(request.PageSize * request.PageNumber - 1)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize);
                
        }
        catch (DbException)
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Não foi buscar categorias");
        }
    }
}