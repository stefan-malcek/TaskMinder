namespace Backend.Application.Common.Models.Filters;

public class SearchPaginationFilterDto : PaginationFilterDto
{
    public string? Search { get; set; }
}
