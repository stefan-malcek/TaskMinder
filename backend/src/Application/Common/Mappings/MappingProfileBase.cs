using System.Reflection;

namespace Backend.Application.Common.Mappings
{
    public abstract class MappingProfileBase : Profile
    {
        protected MappingProfileBase(Assembly assembly)
        {
            ApplyMappingsFromAssembly(assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            ApplyMappingForIMap(assembly, typeof(IMapFrom<>), "IMapFrom`1");
        }

        private void ApplyMappingForIMap(Assembly assembly, Type mapInterface, string mapInterfaceName)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == mapInterface))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface(mapInterfaceName)!.GetMethod("Mapping");

                methodInfo?.Invoke(instance, [this]);
            }
        }
    }
}
