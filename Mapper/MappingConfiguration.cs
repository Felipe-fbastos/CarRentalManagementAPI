using Mapster;

namespace CarRentalManagementAPI.Mapper
{
    public static class MappingConfiguration
    {
        public static IServiceCollection RegisterMaps(this IServiceCollection services)
        {

            services.AddMapster();

            return services;
        }
    }
}
