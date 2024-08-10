using System.Reflection;

namespace Backend.Application.Common.Mappings;

public class AppMappingProfile : MappingProfileBase
{
    public AppMappingProfile() : base(Assembly.GetExecutingAssembly()) { }
}
