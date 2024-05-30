using System.Data.Common;
using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description
        };

        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria criada com sucesso");
        }
        catch (DbUpdateException)
        {
            return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
        }

    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);
            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");
            
            category.UserId = request.UserId;
            category.Title = request.Title;
            category.Description = request.Description;

            context.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria atualizada com sucesso");
        }
        catch (DbUpdateException)
        {
            return new Response<Category?>(null, 500, "Não foi possível atualizar a categoria");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria excluída com sucesso");
        }
        catch (DbUpdateException)
        {
            return new Response<Category?>(null, 500, "Não foi possível excluir a categoria");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);
            return category is null
                ? new Response<Category?>(null, 404, "Categoria não encontrada")
                : new Response<Category?>(category, 201, "Categoria encontrada com sucesso");
        }
        catch (DbException)
        {
            return new Response<Category?>(null, 500, "Não foi buscar a categoria");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context.Categories
                .AsNoTracking()
                .Where(c => c.UserId == request.UserId)
                .OrderBy(c => c.Title);

            var categories = await query
                .Skip(request.PageSize * request.PageNumber - 1)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
                
        }
        catch (DbException)
        {
            return new PagedResponse<List<Category>?>(null, 500, "Não foi buscar categorias");
        }
    }
}