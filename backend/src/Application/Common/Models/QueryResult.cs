namespace Backend.Application.Common.Models;

public class QueryResult<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
}
