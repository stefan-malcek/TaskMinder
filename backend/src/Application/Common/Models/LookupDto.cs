using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;

namespace Backend.Application.Common.Models;

public class LookupDto : IMapFrom<NoteList>
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;
}
