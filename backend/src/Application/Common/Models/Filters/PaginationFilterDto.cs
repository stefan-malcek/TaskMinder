namespace Backend.Application.Common.Models.Filters;

public class PaginationFilterDto
{
    /// <summary>
    /// Items offset
    /// </summary>
    /// <example>0</example>
    public int? Offset { get; set; } = Pagination.DefaultOffset;

    /// <summary>
    /// Page Size
    /// </summary>
    /// <example>10</example>
    public int? Limit { get; set; } = Pagination.DefaultLimit;

    public OrderFilterDto ToOrderFilterDto()
    {
        return new OrderFilterDto { Offset = Offset, Limit = Limit };
    }
}
