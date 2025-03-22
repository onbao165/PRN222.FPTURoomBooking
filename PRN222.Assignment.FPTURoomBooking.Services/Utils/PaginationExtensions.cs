using Mapster;
using Microsoft.EntityFrameworkCore;

namespace PRN222.Assignment.FPTURoomBooking.Services.Utils;

public static class PaginationExtensions
{
    public static async Task<PaginationResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        PaginationParams paginationParams)
    {
        var totalItems = await source.CountAsync();

        var items = await source
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PaginationResult<T>(
            items,
            totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    public static async Task<PaginationResult<TResult>> ProjectToPaginatedListAsync<TSource, TResult>(
        this IQueryable<TSource> source,
        PaginationParams paginationParams)
    {
        var totalItems = await source.CountAsync();

        var items = await source
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ProjectToType<TResult>()
            .ToListAsync();

        return new PaginationResult<TResult>(
            items,
            totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    public static PaginationResult<TResult> MapToPaginationedList<TSource, TResult>(this PaginationResult<TSource> source)
    {
        var items = source.Items.Adapt<IEnumerable<TResult>>();
        return new PaginationResult<TResult>(
            items,
            source.TotalItems,
            source.PageNumber,
            source.PageSize
        );
    }
}