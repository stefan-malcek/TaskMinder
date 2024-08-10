namespace Backend.Application.Common.Models.Filters;
public class OrderFilterDto : PaginationFilterDto
{
    /// <summary>
    /// Sort by column name
    /// </summary>
    public string SortBy { get; set; } = string.Empty;

    /// <summary>
    /// Is sort ascending
    /// </summary>
    public bool IsSortAscending { get; set; } = false;
}
