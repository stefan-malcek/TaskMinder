using Backend.Application.Common.Models;
using Backend.Application.Common.Models.Filters;
using Backend.Domain.Common;

namespace Backend.Application.Common.Extensions;

internal static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PaginationFilterDto filter)
    {
        if (filter.Offset is null or < 0)
        {
            filter.Offset = Pagination.DefaultOffset;
        }

        if (filter.Limit is null or < 0)
        {
            filter.Limit = Pagination.DefaultLimit;
        }

        return query.Skip(filter.Offset.Value).Take(filter.Limit.Value);
    }

    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, OrderFilterDto filter,
        Dictionary<string, SortingHeader<T>> columnsMap) where T : BaseAuditableEntity
    {
        if (string.IsNullOrWhiteSpace(filter.SortBy) || !columnsMap.TryGetValue(filter.SortBy, out SortingHeader<T>? sortingHeader))
        {
            return filter.IsSortAscending
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt);
        }

        var orderedQuery = filter.IsSortAscending
            ? query.OrderBy(sortingHeader.OrderBy)
            : query.OrderByDescending(sortingHeader.OrderBy);

        if (sortingHeader.ThenOrderBy == null)
        {
            return orderedQuery;
        }

        return filter.IsSortAscending
            ? orderedQuery.ThenBy(sortingHeader.ThenOrderBy)
            : orderedQuery.ThenByDescending(sortingHeader.ThenOrderBy);
    }

    public static async Task<QueryResult<T>> ToQueryResultAsync<T>(this IQueryable<T> query, PaginationFilterDto filter, CancellationToken cancellationToken)
    {
        var result = new QueryResult<T> { TotalItems = await query.CountAsync(cancellationToken) };

        var queryable = query.ApplyPaging(filter);

        result.Items = await queryable.ToListAsync(cancellationToken);
        result.Offset = filter.Offset!.Value;
        result.Limit = filter.Limit!.Value;

        return result;
    }

    public static async Task<QueryResult<TR>> ToQueryResultAsync<T, TR>(this IQueryable<T> query, PaginationFilterDto filter, IMapper mapper,
        CancellationToken cancellationToken, object? mapperParams = null)
    {
        var result = new QueryResult<TR> { TotalItems = await query.CountAsync(cancellationToken) };

        var queryable = query.ApplyPaging(filter);

        result.Items = await queryable.ProjectTo<TR>(mapper.ConfigurationProvider, mapperParams).ToListAsync(cancellationToken);
        result.Offset = filter.Offset!.Value;
        result.Limit = filter.Limit!.Value;

        return result;
    }
}
