using System.Linq.Expressions;

namespace Backend.Application.Common.Models.Filters;

internal class SortingHeader<T>(Expression<Func<T, object>> orderBy, Expression<Func<T, object>>? thenOrderBy = null)
{
    public Expression<Func<T, object>> OrderBy { get; set; } = orderBy;
    public Expression<Func<T, object>>? ThenOrderBy { get; set; } = thenOrderBy;
}
